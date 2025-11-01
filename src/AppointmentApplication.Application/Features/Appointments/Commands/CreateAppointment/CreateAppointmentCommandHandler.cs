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
    public class CreateAppointmentCommandHandler(
        ILogger<CreateAppointmentCommandHandler> logger,
        IAppDbContext context)
        : IRequestHandler<CreateAppointmentCommand, Result<AppointmentDto>>
    {
        private readonly ILogger<CreateAppointmentCommandHandler> _logger = logger;
        private readonly IAppDbContext _context = context;

        public async Task<Result<AppointmentDto>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ Validate input parameters

            // 2️⃣ Validate Patient exists and is active
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == request.UserId && p.IsActive, cancellationToken);

            if (patient == null)
            {
                _logger.LogWarning("Appointment creation failed. Patient not found or inactive. PatientId: {PatientId}", request.UserId);
                return ApplicationAppointmentErrors.PatientNotFound(request.UserId);
            }

            // 3️⃣ Validate Doctor exists and is active
            var doctor = await _context.Doctors
                .Include(d => d.Schedules)
                .Include(d => d.ScheduleExceptions)
                .FirstOrDefaultAsync(d => d.Id == request.DoctorId && d.IsActive, cancellationToken);

            if (doctor == null)
            {
                _logger.LogWarning("Appointment creation failed. Doctor not found or inactive. DoctorId: {DoctorId}", request.DoctorId);
                return ApplicationAppointmentErrors.DoctorNotFound(request.DoctorId);
            }

            // 4️⃣ Validate Facility exists and is active
            var facility = await _context.HealthcareFacilities
                .FirstOrDefaultAsync(f => f.Id == request.FacilityId && f.IsActive, cancellationToken);

            if (facility == null)
            {
                _logger.LogWarning("Appointment creation failed. Facility not found or inactive. FacilityId: {FacilityId}", request.FacilityId);
                return ApplicationAppointmentErrors.FacilityNotFound(request.FacilityId);
            }

            // 5️⃣ Check if doctor is associated with the facility
            if (doctor.FacilityId != request.FacilityId)
            {
                _logger.LogWarning(
                    "Appointment creation failed. Doctor {DoctorId} is not associated with Facility {FacilityId}",
                    request.DoctorId, request.FacilityId);
                return ApplicationAppointmentErrors.DoctorNotInFacility(request.DoctorId, request.FacilityId);
            }

            // 6️⃣ Check for scheduling conflicts
            // Check if doctor has schedule for the requested day
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

            // 7️⃣ Create Appointment domain entity
            Result<Appointment> createAppointmentResult;

            createAppointmentResult = Appointment.CreateWithBilling(
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

            // 8️⃣ Save to database
            _context.Appointments.Add(appointment);

            if (appointment.Billing != null)
            {
                _context.Billings.Add(appointment.Billing);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // 9️⃣ Reload the appointment with all related data to ensure we have complete data for DTO mapping
            var createdAppointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Facility)
                .Include(a => a.Billing)
                .FirstOrDefaultAsync(a => a.Id == appointment.Id, cancellationToken);

            // if (createdAppointment == null)
            // {
            //     _logger.LogError("Failed to reload created appointment. AppointmentId: {AppointmentId}", appointment.Id);
            //     return ApplicationAppointmentErrors.CreateAppointmentFailed("Failed to reload created appointment. AppointmentId: " + appointment.Id);
            // }

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
    }
}