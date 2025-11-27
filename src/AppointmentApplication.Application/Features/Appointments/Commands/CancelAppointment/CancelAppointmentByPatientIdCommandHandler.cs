using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Appointments.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Appointments.Enums;
using AppointmentApplication.Domain.Billings.Enums;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Appointments.Commands.CancelAppointment
{
    public class CancelAppointmentByPatientIdCommandHandler
        : IRequestHandler<CancelAppointmentByPatientIdCommand, Result<Updated>>
    {
        private readonly ILogger<CancelAppointmentByPatientIdCommandHandler> _logger;
        private readonly IAppDbContext _context;
        private readonly IAppointmentEmailService _emailService;
        private readonly INotificationService _notificationService;

        public CancelAppointmentByPatientIdCommandHandler(
            ILogger<CancelAppointmentByPatientIdCommandHandler> logger,
            IAppDbContext context,
            IAppointmentEmailService emailService,
            INotificationService notificationService)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
            _notificationService = notificationService;
        }

        public async Task<Result<Updated>> Handle(
            CancelAppointmentByPatientIdCommand request,
            CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                .Include(a => a.Facility)
                .Include(a => a.Billing)
                .FirstOrDefaultAsync(
                    a => a.Id == request.AppointmentId,
                    cancellationToken);

            if (appointment == null)
            {
                _logger.LogWarning(
                    "Appointment not found for cancellation. AppointmentId: {AppointmentId}, PatientId: {PatientId}",
                    request.AppointmentId,
                    request.UserId);

                return ApplicationAppointmentErrors.AppointmentNotFound(request.AppointmentId);
            }

            // Ownership is guaranteed by query filter; treat any mismatch as unauthorized.
            if (appointment.Patient.UserId != request.UserId)
            {
                _logger.LogWarning(
                    "Patient {PatientId} is not authorized to cancel appointment {AppointmentId}",
                    request.UserId,
                    request.AppointmentId);

                return ApplicationAppointmentErrors.UnauthorizedToCancelAppointment(request.AppointmentId);
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} is already cancelled.",
                    request.AppointmentId);

                return ApplicationAppointmentErrors.AppointmentAlreadyCancelled(request.AppointmentId);
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} cannot be cancelled because it's already completed.",
                    request.AppointmentId);

                return ApplicationAppointmentErrors.CannotCancelCompletedAppointment(request.AppointmentId);
            }

            var appointmentDateTime = appointment.ScheduledDate.ToDateTime(
                TimeOnly.FromTimeSpan(appointment.ScheduledTime));

            if (appointmentDateTime < DateTime.UtcNow)
            {
                _logger.LogWarning(
                    "Cannot cancel past appointment. AppointmentId: {AppointmentId}, Scheduled: {ScheduledDateTime}",
                    request.AppointmentId,
                    appointmentDateTime);

                return ApplicationAppointmentErrors.CannotCancelPastAppointment(appointment.ScheduledDate);
            }

            var timeUntilAppointment = appointmentDateTime - DateTime.UtcNow;
            if (timeUntilAppointment <= TimeSpan.FromHours(24) && timeUntilAppointment > TimeSpan.Zero)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} cannot be cancelled within 24 hours of scheduled time.",
                    request.AppointmentId);

                return ApplicationAppointmentErrors.CannotCancelWithin24Hours(request.AppointmentId);
            }

            var cancelResult = CancelAppointmentAndBilling(appointment, request.CancellationReason);
            if (cancelResult.IsError)
            {
                _logger.LogWarning(
                    "Failed to cancel appointment {AppointmentId}: {Errors}",
                    request.AppointmentId,
                    string.Join(", ", cancelResult.Errors));

                return cancelResult.Errors;
            }

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Appointment cancelled successfully by patient. AppointmentId: {AppointmentId}, PatientId: {PatientId}",
                appointment.Id,
                request.UserId);

            _ = Task.Run(async () =>
            {
                try
                {
                    await _emailService.SendAppointmentCancelledEmailAsync(appointment, request.CancellationReason);
                }
                catch (Exception emailEx)
                {
                    _logger.LogError(emailEx, "Background email sending failed for appointment cancellation");
                }
            });

            var doctorName = $"{appointment.Doctor.FirstName} {appointment.Doctor.LastName}";
            await _notificationService.NotifyAppointmentCancelledAsync(
                appointment.Patient.UserId,
                appointment.Id,
                doctorName,
                request.CancellationReason);

            return Result.Updated;
        }

        private Result<Updated> CancelAppointmentAndBilling(Appointment appointment, string cancellationReason)
        {
            var cancelMethod = appointment.GetType().GetMethod("Cancel");
            if (cancelMethod != null)
            {
                var result = cancelMethod.Invoke(appointment, new object[] { cancellationReason }) as Result<Updated>;
                if (result != null && result.IsError)
                {
                    return result;
                }
            }
            else
            {
                var statusProperty = appointment.GetType().GetProperty("Status");
                var cancellationReasonProperty = appointment.GetType().GetProperty("CancellationReason");
                var updatedAtProperty = appointment.GetType().GetProperty("UpdatedAtUtc");

                statusProperty?.SetValue(appointment, AppointmentStatus.Cancelled);
                cancellationReasonProperty?.SetValue(appointment, cancellationReason.Trim());
                updatedAtProperty?.SetValue(appointment, DateTime.UtcNow);
            }

            if (appointment.Billing != null)
            {
                CancelBilling(appointment.Billing);
            }

            return Result.Updated;
        }

        private void CancelBilling(Domain.Billings.Billing billing)
        {
            var statusProperty = billing.GetType().GetProperty("Status");
            var updatedAtProperty = billing.GetType().GetProperty("UpdatedAtUtc");

            if (statusProperty == null || !statusProperty.CanWrite)
            {
                return;
            }

            var currentStatus = (BillingStatus?)statusProperty.GetValue(billing);
            if (currentStatus.HasValue &&
                (currentStatus.Value == BillingStatus.Pending || currentStatus.Value == BillingStatus.Overdue))
            {
                statusProperty.SetValue(billing, BillingStatus.Cancelled);

                updatedAtProperty?.SetValue(billing, DateTime.UtcNow);

                _logger.LogInformation(
                    "Billing cancelled. BillingId: {BillingId}, PreviousStatus: {PreviousStatus}",
                    billing.Id,
                    currentStatus.Value);
            }
            else
            {
                _logger.LogWarning(
                    "Billing cannot be cancelled. BillingId: {BillingId}, CurrentStatus: {CurrentStatus}",
                    billing.Id,
                    currentStatus?.ToString() ?? "Unknown");
            }
        }
    }
}
