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
    public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, Result<Updated>>
    {
        private readonly ILogger<CancelAppointmentCommandHandler> _logger;
        private readonly IAppDbContext _context;

        public CancelAppointmentCommandHandler(
            ILogger<CancelAppointmentCommandHandler> logger,
            IAppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result<Updated>> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            // 1. Find the appointment with related doctor and patient
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Billing)
                .FirstOrDefaultAsync(a => a.Id == request.AppointmentId && a.Doctor.UserId == request.UserId, cancellationToken);

            if (appointment == null)
            {
                _logger.LogWarning("Appointment not found. AppointmentId: {AppointmentId}", request.AppointmentId);
                return ApplicationAppointmentErrors.AppointmentNotFound(request.AppointmentId);
            }

            // 2. Validate appointment can be cancelled
            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} is already cancelled.",
                    request.AppointmentId);
                return ApplicationAppointmentErrors.AppointmentAlreadyCancelled(request.AppointmentId);
            }
                        // 2. Validate appointment can be cancelled
            if (appointment.Status == AppointmentStatus.Confirmed)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} is already cancelled.",
                    request.AppointmentId);
                return ApplicationAppointmentErrors.AppointmentAlreadyConfirmed(request.AppointmentId);
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} cannot be cancelled because it's already completed.",
                    request.AppointmentId);
                return ApplicationAppointmentErrors.CannotCancelCompletedAppointment(request.AppointmentId);
            }

            // 3. Check if appointment is too close to scheduled time (within 24 hours)
            var scheduledTime = TimeOnly.FromTimeSpan(appointment.ScheduledTime);
            var appointmentDateTime = appointment.ScheduledDate.ToDateTime(scheduledTime);
            var timeUntilAppointment = appointmentDateTime - DateTime.UtcNow;

            if (timeUntilAppointment <= TimeSpan.FromHours(24) && timeUntilAppointment > TimeSpan.Zero)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} cannot be cancelled within 24 hours of scheduled time.",
                    request.AppointmentId);
                return ApplicationAppointmentErrors.CannotCancelWithin24Hours(request.AppointmentId);
            }

            // 4. Check if appointment is in the past
            if (appointmentDateTime < DateTime.UtcNow)
            {
                _logger.LogWarning(
                    "Cannot cancel past appointment. AppointmentId: {AppointmentId}, Scheduled: {ScheduledDateTime}",
                    request.AppointmentId, appointmentDateTime);
                return ApplicationAppointmentErrors.CannotCancelPastAppointment(appointment.ScheduledDate);
            }

            // 5. Cancel the appointment and billing
            var cancelResult = CancelAppointmentAndBilling(appointment, request.CancellationReason);
            if (cancelResult.IsError)
            {
                _logger.LogWarning(
                    "Failed to cancel appointment {AppointmentId}: {Errors}",
                    request.AppointmentId, string.Join(", ", cancelResult.Errors));
                return cancelResult.Errors;
            }

            // 6. Save changes
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Appointment cancelled successfully. AppointmentId: {AppointmentId}, " +
                "CancelledBy: {UserId}, Reason: {CancellationReason}",
                appointment.Id, request.UserId, request.CancellationReason);

            return Result.Updated;
        }

        private Result<Updated> CancelAppointmentAndBilling(Appointment appointment, string cancellationReason)
        {
            // Update appointment status using reflection
            var statusProperty = appointment.GetType().GetProperty("Status");
            var cancellationReasonProperty = appointment.GetType().GetProperty("CancellationReason");
            var updatedAtProperty = appointment.GetType().GetProperty("UpdatedAtUtc");

            if (statusProperty != null && statusProperty.CanWrite)
            {
                statusProperty.SetValue(appointment, AppointmentStatus.Cancelled);
            }

            if (cancellationReasonProperty != null && cancellationReasonProperty.CanWrite)
            {
                cancellationReasonProperty.SetValue(appointment, cancellationReason.Trim());
            }

            if (updatedAtProperty != null && updatedAtProperty.CanWrite)
            {
                updatedAtProperty.SetValue(appointment, DateTime.UtcNow);
            }

            // Cancel associated billing if exists
            if (appointment.Billing != null)
            {
                CancelBilling(appointment.Billing);
            }

            return Result.Updated;
        }

        private void CancelBilling(Domain.Billings.Billing billing)
        {
            // Get billing properties using reflection
            var statusProperty = billing.GetType().GetProperty("Status");
            var updatedAtProperty = billing.GetType().GetProperty("UpdatedAtUtc");

            if (statusProperty != null && statusProperty.CanWrite)
            {
                // Only cancel billing if it's in a cancellable state
                var currentStatus = (BillingStatus?)statusProperty.GetValue(billing);

                if (currentStatus.HasValue &&
                   (currentStatus.Value == BillingStatus.Pending ||
                    currentStatus.Value == BillingStatus.Overdue))
                {
                    statusProperty.SetValue(billing, BillingStatus.Cancelled);

                    _logger.LogInformation(
                        "Billing cancelled. BillingId: {BillingId}, PreviousStatus: {PreviousStatus}",
                        billing.Id, currentStatus.Value);

                    // Update billing timestamp
                    if (updatedAtProperty != null && updatedAtProperty.CanWrite)
                    {
                        updatedAtProperty.SetValue(billing, DateTime.UtcNow);
                    }
                }
                else
                {
                    _logger.LogWarning(
                        "Billing cannot be cancelled. BillingId: {BillingId}, CurrentStatus: {CurrentStatus}",
                        billing.Id, currentStatus?.ToString() ?? "Unknown");
                }
            }
        }
    }
}