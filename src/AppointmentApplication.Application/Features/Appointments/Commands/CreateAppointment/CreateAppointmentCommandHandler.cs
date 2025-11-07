using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Appointments.Dtos;
using AppointmentApplication.Application.Features.Appointments.Errors;
using AppointmentApplication.Application.Features.Appointments.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Appointments.Enums;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Patients;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Result<AppointmentDto>>
    {
        private readonly ILogger<CreateAppointmentCommandHandler> _logger;
        private readonly IAppDbContext _context;
        private readonly IAppointmentEmailService _emailService;
        private readonly INotificationService _notificationService;

        public CreateAppointmentCommandHandler(
            ILogger<CreateAppointmentCommandHandler> logger,
            IAppDbContext context,
            IAppointmentEmailService emailService,
            INotificationService notificationService)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
            _notificationService = notificationService;
        }
        public async Task<Result<AppointmentDto>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1️⃣ Validate Patient exists and is active
                var patient = await _context.Patients
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.UserId == request.UserId && p.IsActive, cancellationToken);

                if (patient == null)
                {
                    _logger.LogWarning("Appointment creation failed. Patient not found or inactive. PatientId: {PatientId}", request.UserId);
                    return ApplicationAppointmentErrors.PatientNotFound(request.UserId);
                }

                // 2️⃣ Validate Doctor exists and is active
                var doctor = await _context.Doctors
                    .Include(d => d.Schedules)
                    .Include(d => d.ScheduleExceptions)
                    .FirstOrDefaultAsync(d => d.Id == request.DoctorId && d.IsActive, cancellationToken);

                if (doctor == null)
                {
                    _logger.LogWarning("Appointment creation failed. Doctor not found or inactive. DoctorId: {DoctorId}", request.DoctorId);
                    return ApplicationAppointmentErrors.DoctorNotFound(request.DoctorId);
                }

                // 3️⃣ Validate Facility exists and is active
                var facility = await _context.HealthcareFacilities
                    .FirstOrDefaultAsync(f => f.Id == request.FacilityId && f.IsActive, cancellationToken);

                if (facility == null)
                {
                    _logger.LogWarning("Appointment creation failed. Facility not found or inactive. FacilityId: {FacilityId}", request.FacilityId);
                    return ApplicationAppointmentErrors.FacilityNotFound(request.FacilityId);
                }

                // 4️⃣ Check if doctor is associated with the facility
                if (doctor.FacilityId != request.FacilityId)
                {
                    _logger.LogWarning(
                        "Appointment creation failed. Doctor {DoctorId} is not associated with Facility {FacilityId}",
                        request.DoctorId, request.FacilityId);
                    return ApplicationAppointmentErrors.DoctorNotInFacility(request.DoctorId, request.FacilityId);
                }

                // 5️⃣ Check for scheduling conflicts
                var dayOfWeek = request.ScheduledDate.DayOfWeek;
                var hasSchedule = doctor.Schedules.Any(s =>
                    s.IsAvailable &&
                    request.ScheduledTime >= s.StartTime &&
                    request.ScheduledTime.Add(TimeSpan.FromMinutes(request.DurationMinutes)) <= s.EndTime);

                if (!hasSchedule)
                {
                    _logger.LogWarning(
                        "Doctor {DoctorId} has no schedule for {DayOfWeek} at {ScheduledTime}",
                        request.DoctorId, dayOfWeek, request.ScheduledTime);
                    return ApplicationAppointmentErrors.DoctorNotAvailable(request.DoctorId, request.ScheduledDate, request.ScheduledTime);
                }

                // Check for schedule exceptions
                var hasException = doctor.ScheduleExceptions.Any(se =>
                    se.Date == request.ScheduledDate &&
                    request.ScheduledTime >= se.StartTime &&
                    request.ScheduledTime.Add(TimeSpan.FromMinutes(request.DurationMinutes)) <= se.EndTime);

                if (hasException)
                {
                    _logger.LogWarning(
                        "Doctor {DoctorId} has schedule exception on {ScheduledDate}",
                        request.DoctorId, request.ScheduledDate);
                    return ApplicationAppointmentErrors.DoctorHasException(request.DoctorId, request.ScheduledDate);
                }

                // Check for existing appointments at the same time
                var existingAppointments = await _context.Appointments
                    .Where(a => a.DoctorId == request.DoctorId &&
                               a.ScheduledDate == request.ScheduledDate &&
                               a.Status != AppointmentStatus.Cancelled &&
                               a.Status != AppointmentStatus.NoShow)
                    .ToListAsync(cancellationToken);

                var newAppointmentEndTime = request.ScheduledTime.Add(TimeSpan.FromMinutes(request.DurationMinutes));

                var hasConflict = existingAppointments.Any(existing =>
                {
                    var existingEndTime = existing.ScheduledTime.Add(TimeSpan.FromMinutes(existing.DurationMinutes));
                    return request.ScheduledTime < existingEndTime && existing.ScheduledTime < newAppointmentEndTime;
                });

                if (hasConflict)
                {
                    _logger.LogWarning(
                        "Appointment conflict for Doctor {DoctorId} on {ScheduledDate} at {ScheduledTime}",
                        request.DoctorId, request.ScheduledDate, request.ScheduledTime);
                    return ApplicationAppointmentErrors.AppointmentConflict(request.DoctorId, request.ScheduledDate, request.ScheduledTime);
                }

                // 6️⃣ Create Appointment domain entity
                Result<Appointment> createAppointmentResult = Appointment.CreateWithBilling(
                   patient.Id,
                   request.DoctorId,
                   request.FacilityId,
                   request.ScheduledDate,
                   request.ScheduledTime,
                   request.DurationMinutes,
                   200);

                if (createAppointmentResult.IsError)
                {
                    _logger.LogWarning("Appointment creation failed: {Errors}", string.Join(", ", createAppointmentResult.Errors));
                    return createAppointmentResult.Errors;
                }

                var appointment = createAppointmentResult.Value;

                // 7️⃣ Save to database
                _context.Appointments.Add(appointment);
                if (appointment.Billing != null)
                {
                    _context.Billings.Add(appointment.Billing);
                }

                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Appointment saved successfully to database. AppointmentId: {AppointmentId}", appointment.Id);

                // 8️⃣ Reload the appointment with all related data
                var createdAppointment = await _context.Appointments
                   .Include(a => a.Patient)
                       .ThenInclude(p => p.User)
                   .Include(a => a.Doctor)
                       .ThenInclude(d => d.User)
                   .Include(a => a.Facility)
                   .Include(a => a.Billing)
                   .FirstOrDefaultAsync(a => a.Id == appointment.Id, cancellationToken);

                if (createdAppointment == null)
                {
                    _logger.LogError("Failed to reload created appointment. AppointmentId: {AppointmentId}", appointment.Id);
                    return ApplicationAppointmentErrors.CreateAppointmentFailed("Failed to reload created appointment.");
                }

                try
                {
                    // Send email to patient
                    await _emailService.SendAppointmentCreatedEmailAsync(createdAppointment);

                    // Send real-time notification to doctor
                    var doctorUserId = createdAppointment.Doctor.UserId;
                    var patientName = $"{createdAppointment.Patient.FirstName} {createdAppointment.Patient.LastName}";
                    var scheduledDateTime = createdAppointment.ScheduledDate.ToDateTime(TimeOnly.FromTimeSpan(createdAppointment.ScheduledTime));

                    await _notificationService.NotifyAppointmentCreatedAsync(
                        doctorUserId,
                        createdAppointment.Id,
                        patientName,
                        scheduledDateTime,
                        createdAppointment.ScheduledTime);

                    _logger.LogInformation("Notifications sent successfully for appointment {AppointmentId}", createdAppointment.Id);
                }
                catch (Exception notificationEx)
                {
                    _logger.LogError(notificationEx, "Notification sending failed for appointment {AppointmentId}", createdAppointment.Id);
                    // Don't fail the entire appointment creation if notifications fail
                }

                // 🔟 Convert to DTO
                var appointmentDto = createdAppointment.ToDto();

                _logger.LogInformation(
                    "Appointment created successfully. AppointmentId: {AppointmentId}, Patient: {PatientName}, Doctor: {DoctorName}, Date: {ScheduledDate}",
                    appointment.Id,
                    $"{createdAppointment.Patient?.FirstName} {createdAppointment.Patient?.LastName}",
                    $"{createdAppointment.Doctor?.FirstName} {createdAppointment.Doctor?.LastName}",
                    request.ScheduledDate);

                return appointmentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during appointment creation");
                return ApplicationAppointmentErrors.CreateAppointmentFailed($"Unexpected error: {ex.Message}");
            }
        }
    }
}