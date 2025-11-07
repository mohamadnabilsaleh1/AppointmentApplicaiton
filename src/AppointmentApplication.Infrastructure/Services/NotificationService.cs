using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Notifications.Commands.CreateAppointmentCommand;

using AppointmentApplication.Application.Shared.Interfaces;

using MediatR;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMediator _mediator;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            IHubContext<NotificationHub> hubContext,
            IMediator mediator,
            ILogger<NotificationService> logger)
        {
            _hubContext = hubContext;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task NotifyAppointmentCreatedAsync(Guid doctorUserId, Guid appointmentId, string patientName, DateTime scheduledDate, TimeSpan scheduledTime)
        {
            try
            {
                var title = "📅 New Appointment Request";

                // Convert TimeSpan to DateTime for proper AM/PM formatting
                var timeWithAmPm = DateTime.Today.Add(scheduledTime).ToString("hh:mm tt");
                var message = $"You have a new appointment request from {patientName} on {scheduledDate:MMM dd, yyyy} at {timeWithAmPm}";

                // 1. Save to database
                var command = new CreateAppointmentNotificationCommand(
                    doctorUserId, title, message, "APPOINTMENT_CREATED", appointmentId);

                await _mediator.Send(command);

                // 2. Send real-time notification
                var notificationData = new
                {
                    Type = "APPOINTMENT_CREATED",
                    AppointmentId = appointmentId,
                    PatientName = patientName,
                    ScheduledDate = scheduledDate,
                    ScheduledTime = scheduledTime,
                    Title = title,
                    Message = message,
                    Timestamp = DateTime.UtcNow,
                    IsRead = false
                };

                await _hubContext.Clients.Group($"user-{doctorUserId}")
                    .SendAsync("ReceiveNotification", notificationData);

                _logger.LogInformation("Appointment creation notification sent to doctor {DoctorUserId}", doctorUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send appointment creation notification to doctor {DoctorUserId}", doctorUserId);
            }
        }

        public async Task NotifyAppointmentCancelledAsync(Guid patientUserId, Guid appointmentId, string doctorName, string reason)
        {
            // Implementation for cancellation notifications
        }

        public async Task NotifyAppointmentConfirmedAsync(Guid patientUserId, Guid appointmentId, string doctorName)
        {
            // Implementation for confirmation notifications
        }

        public async Task NotifyAppointmentCompletedAsync(Guid patientUserId, Guid appointmentId, string doctorName)
        {
            // Implementation for completion notifications
        }
    }
}