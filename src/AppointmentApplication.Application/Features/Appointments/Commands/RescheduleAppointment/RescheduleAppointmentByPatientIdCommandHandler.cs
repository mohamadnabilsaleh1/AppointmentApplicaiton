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

namespace AppointmentApplication.Application.Features.Appointments.Commands.RescheduleAppointment
{
    public class RescheduleAppointmentByPatientIdCommandHandler
        : IRequestHandler<RescheduleAppointmentByPatientIdCommand, Result<Updated>>
    {
        private readonly ILogger<RescheduleAppointmentByPatientIdCommandHandler> _logger;
        private readonly IAppDbContext _context;

        public RescheduleAppointmentByPatientIdCommandHandler(
            ILogger<RescheduleAppointmentByPatientIdCommandHandler> logger,
            IAppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result<Updated>> Handle(
            RescheduleAppointmentByPatientIdCommand request,
            CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(
                    a => a.Id == request.AppointmentId,
                    cancellationToken);

            if (appointment == null)
            {
                _logger.LogWarning(
                    "Appointment not found for rescheduling. AppointmentId: {AppointmentId}, PatientId: {PatientId}",
                    request.AppointmentId,
                    request.UserId);

                return ApplicationAppointmentErrors.AppointmentNotFound(request.AppointmentId);
            }

            if (appointment.Patient.UserId != request.UserId)
            {
                _logger.LogWarning(
                    "Patient {PatientId} is not authorized to reschedule appointment {AppointmentId}",
                    request.UserId,
                    request.AppointmentId);

                return ApplicationAppointmentErrors.UnauthorizedToCancelAppointment(request.AppointmentId);
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} is already cancelled; cannot reschedule.",
                    request.AppointmentId);

                return ApplicationAppointmentErrors.CannotRescheduleCompleted;
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} cannot be rescheduled because it's completed.",
                    request.AppointmentId);

                return ApplicationAppointmentErrors.CannotRescheduleCompleted;
            }

            var originalAppointmentDateTime = appointment.ScheduledDate.ToDateTime(
                TimeOnly.FromTimeSpan(appointment.ScheduledTime));

            if (originalAppointmentDateTime < DateTime.UtcNow)
            {
                _logger.LogWarning(
                    "Cannot reschedule past appointment. AppointmentId: {AppointmentId}, Scheduled: {ScheduledDateTime}",
                    request.AppointmentId,
                    originalAppointmentDateTime);

                return ApplicationAppointmentErrors.CannotReschedulePastAppointment(appointment.ScheduledDate);
            }

            var timeUntilAppointment = originalAppointmentDateTime - DateTime.UtcNow;
            if (timeUntilAppointment <= TimeSpan.FromHours(24) && timeUntilAppointment > TimeSpan.Zero)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} cannot be rescheduled within 24 hours of scheduled time.",
                    request.AppointmentId);

                return ApplicationAppointmentErrors.CannotRescheduleWithin24Hours(request.AppointmentId);
            }

            var rescheduleResult = appointment.Reschedule(request.NewDate, request.NewTime);
            if (rescheduleResult.IsError)
            {
                _logger.LogWarning(
                    "Failed to reschedule appointment {AppointmentId}: {Errors}",
                    request.AppointmentId,
                    string.Join(", ", rescheduleResult.Errors));

                return rescheduleResult.Errors;
            }

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Appointment rescheduled successfully by patient. AppointmentId: {AppointmentId}, PatientId: {PatientId}, NewDate: {NewDate}, NewTime: {NewTime}",
                appointment.Id,
                request.UserId,
                request.NewDate,
                request.NewTime);

            return Result.Updated;
        }
    }
}
