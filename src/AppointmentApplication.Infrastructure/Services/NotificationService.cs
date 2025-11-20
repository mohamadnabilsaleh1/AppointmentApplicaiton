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
            try
            {
                var title = "❌ Appointment Cancelled";
                var message = string.IsNullOrEmpty(reason)
                    ? $"Your appointment with Dr. {doctorName} has been cancelled."
                    : $"Your appointment with Dr. {doctorName} has been cancelled. Reason: {reason}";

                // 1. Save to database
                var command = new CreateAppointmentNotificationCommand(
                    patientUserId, title, message, "APPOINTMENT_CANCELLED", appointmentId);

                await _mediator.Send(command);

                // 2. Send real-time notification
                var notificationData = new
                {
                    Type = "APPOINTMENT_CANCELLED",
                    AppointmentId = appointmentId,
                    DoctorName = doctorName,
                    Reason = reason,
                    Title = title,
                    Message = message,
                    Timestamp = DateTime.UtcNow,
                    IsRead = false
                };

                await _hubContext.Clients.Group($"user-{patientUserId}")
                    .SendAsync("ReceiveNotification", notificationData);

                _logger.LogInformation("Appointment cancellation notification sent to patient {PatientUserId}", patientUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send appointment cancellation notification to patient {PatientUserId}", patientUserId);
            }
        }

        public async Task NotifyAppointmentConfirmedAsync(Guid patientUserId, Guid appointmentId, string doctorName)
        {
            try
            {
                var title = "✅ Appointment Confirmed";
                var message = $"Your appointment with Dr. {doctorName} has been confirmed.";

                // 1. Save to database
                var command = new CreateAppointmentNotificationCommand(
                    patientUserId, title, message, "APPOINTMENT_CONFIRMED", appointmentId);

                await _mediator.Send(command);

                // 2. Send real-time notification
                var notificationData = new
                {
                    Type = "APPOINTMENT_CONFIRMED",
                    AppointmentId = appointmentId,
                    DoctorName = doctorName,
                    Title = title,
                    Message = message,
                    Timestamp = DateTime.UtcNow,
                    IsRead = false
                };

                await _hubContext.Clients.Group($"user-{patientUserId}")
                    .SendAsync("ReceiveNotification", notificationData);

                _logger.LogInformation("Appointment confirmation notification sent to patient {PatientUserId}", patientUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send appointment confirmation notification to patient {PatientUserId}", patientUserId);
            }
        }

        public async Task NotifyAppointmentCompletedAsync(Guid patientUserId, Guid appointmentId, string doctorName)
        {
            try
            {
                var title = "🎉 Appointment Completed";
                var message = $"Your appointment with Dr. {doctorName} has been marked as completed. Thank you for choosing our service!";

                // 1. Save to database
                var command = new CreateAppointmentNotificationCommand(
                    patientUserId, title, message, "APPOINTMENT_COMPLETED", appointmentId);

                await _mediator.Send(command);

                // 2. Send real-time notification
                var notificationData = new
                {
                    Type = "APPOINTMENT_COMPLETED",
                    AppointmentId = appointmentId,
                    DoctorName = doctorName,
                    Title = title,
                    Message = message,
                    Timestamp = DateTime.UtcNow,
                    IsRead = false
                };

                await _hubContext.Clients.Group($"user-{patientUserId}")
                    .SendAsync("ReceiveNotification", notificationData);

                _logger.LogInformation("Appointment completion notification sent to patient {PatientUserId}", patientUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send appointment completion notification to patient {PatientUserId}", patientUserId);
            }
        }

        // Additional helper method for doctor notifications
        public async Task NotifyAppointmentReminderAsync(Guid userId, Guid appointmentId, string otherPartyName, DateTime scheduledDate, TimeSpan scheduledTime, bool isForPatient = true)
        {
            try
            {
                var title = "⏰ Appointment Reminder";
                var timeWithAmPm = DateTime.Today.Add(scheduledTime).ToString("hh:mm tt");

                var message = isForPatient
                    ? $"Reminder: You have an appointment with Dr. {otherPartyName} on {scheduledDate:MMM dd, yyyy} at {timeWithAmPm}"
                    : $"Reminder: You have an appointment with patient {otherPartyName} on {scheduledDate:MMM dd, yyyy} at {timeWithAmPm}";

                var notificationType = isForPatient ? "APPOINTMENT_REMINDER_PATIENT" : "APPOINTMENT_REMINDER_DOCTOR";

                // 1. Save to database
                var command = new CreateAppointmentNotificationCommand(
                    userId, title, message, notificationType, appointmentId);

                await _mediator.Send(command);

                // 2. Send real-time notification
                var notificationData = new
                {
                    Type = notificationType,
                    AppointmentId = appointmentId,
                    OtherPartyName = otherPartyName,
                    ScheduledDate = scheduledDate,
                    ScheduledTime = scheduledTime,
                    Title = title,
                    Message = message,
                    Timestamp = DateTime.UtcNow,
                    IsRead = false,
                    IsReminder = true
                };

                await _hubContext.Clients.Group($"user-{userId}")
                    .SendAsync("ReceiveNotification", notificationData);

                _logger.LogInformation("Appointment reminder notification sent to user {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send appointment reminder notification to user {UserId}", userId);
            }
        }
    }
}