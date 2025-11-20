using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Appointments.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Appointments.Enums;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Appointments.Commands.ConfirmAppointment
{
    public class ConfirmAppointmentCommandHandler : IRequestHandler<ConfirmAppointmentCommand, Result<Updated>>
    {
        private readonly ILogger<ConfirmAppointmentCommandHandler> _logger;
        private readonly IAppDbContext _context;
        private readonly IAppointmentEmailService _emailService;
        private readonly INotificationService _notificationService;

        public ConfirmAppointmentCommandHandler(
            ILogger<ConfirmAppointmentCommandHandler> logger,
            IAppDbContext context,
            IAppointmentEmailService emailService,
            INotificationService notificationService)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
            _notificationService = notificationService;
        }

        public async Task<Result<Updated>> Handle(ConfirmAppointmentCommand request, CancellationToken cancellationToken)
        {
            // 1. Find the appointment with related doctor and patient
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                .Include(a => a.Facility)
                .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                _logger.LogWarning("Appointment not found. AppointmentId: {AppointmentId}", request.AppointmentId);
                return ApplicationAppointmentErrors.AppointmentNotFound(request.AppointmentId);
            }

            // 2. Verify the user has permission to confirm this appointment
            if (appointment.Doctor.UserId != request.UserId)
            {
                _logger.LogWarning(
                    "User {UserId} is not authorized to confirm appointment {AppointmentId}. Assigned doctor: {DoctorId}",
                    request.UserId, request.AppointmentId, appointment.Doctor.Id);
                return ApplicationAppointmentErrors.UnauthorizedToConfirmAppointment(request.AppointmentId);
            }

            // 3. Validate appointment can be confirmed
            if (appointment.Status != AppointmentStatus.Pending)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} cannot be confirmed. Current status: {Status}",
                    request.AppointmentId, appointment.Status);
                return ApplicationAppointmentErrors.CannotConfirmAppointment(appointment.Status);
            }

            // 4. Check if appointment is not in the past
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (appointment.ScheduledDate < today)
            {
                _logger.LogWarning(
                    "Cannot confirm past appointment. AppointmentId: {AppointmentId}, ScheduledDate: {ScheduledDate}",
                    request.AppointmentId, appointment.ScheduledDate);
                return ApplicationAppointmentErrors.CannotConfirmPastAppointment(appointment.ScheduledDate);
            }

            // 5. Confirm the appointment using domain method
            var confirmResult = appointment.Confirm();
            if (confirmResult.IsError)
            {
                _logger.LogWarning(
                    "Failed to confirm appointment {AppointmentId}: {Errors}",
                    request.AppointmentId, string.Join(", ", confirmResult.Errors));
                return confirmResult.Errors;
            }

            // 6. Save changes
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Appointment confirmed successfully. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}, ScheduledDate: {ScheduledDate}",
                appointment.Id, appointment.Doctor.Id, appointment.ScheduledDate);

            // 7. Send confirmation email ASYNCHRONOUSLY
            _ = Task.Run(async () =>
            {
                try
                {
                    await _emailService.SendAppointmentConfirmedEmailAsync(appointment);
                }
                catch (Exception emailEx)
                {
                    _logger.LogError(emailEx, "❌ Background email sending failed for appointment confirmation");
                }
            });
            var patientUserId = appointment.Patient.UserId;
            var doctorName = $"{appointment.Doctor.FirstName} {appointment.Doctor.LastName}";
            var scheduledDateTime = appointment.ScheduledDate.ToDateTime(TimeOnly.FromTimeSpan(appointment.ScheduledTime));

            await _notificationService.NotifyAppointmentConfirmedAsync(
                patientUserId,
                appointment.Id,
                doctorName);

            _logger.LogInformation("Notifications sent successfully for appointment {AppointmentId}", appointment.Id);

            return Result.Updated;
        }
    }
}