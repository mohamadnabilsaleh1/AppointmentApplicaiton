// IAppointmentEmailService.cs
using System.Threading.Tasks;

using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Appointments;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Appointments;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Shared.Services
{
    public class AppointmentEmailService : IAppointmentEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AppointmentEmailService> _logger;

        public AppointmentEmailService(IEmailSender emailSender, ILogger<AppointmentEmailService> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task SendAppointmentCreatedEmailAsync(Appointment appointment)
        {
            try
            {
                var patientEmail = appointment.Patient?.User?.Email ?? "default@email.com";
                var patientName = appointment.Patient?.FirstName ?? "Patient";
                var doctorName = $"{appointment.Doctor?.FirstName} {appointment.Doctor?.LastName}".Trim();
                var facilityName = appointment.Facility?.Name ?? "Medical Facility";
                var formattedTime = FormatTimeSpan(appointment.ScheduledTime);

                var subject = "🎯 Your Appointment Has Been Created - Pending Confirmation";
                var body = $@"
                    <h2>Appointment Created Successfully</h2>
                    <p>Dear {patientName},</p>
                    <p>Your appointment has been created and is pending confirmation from Dr. {doctorName}.</p>
                    <p><strong>Appointment Details:</strong></p>
                    <ul>
                        <li><strong>Doctor:</strong> Dr. {doctorName}</li>
                        <li><strong>Date:</strong> {appointment.ScheduledDate:dddd, MMMM dd, yyyy}</li>
                        <li><strong>Time:</strong> {formattedTime}</li>
                        <li><strong>Duration:</strong> {appointment.DurationMinutes} minutes</li>
                        <li><strong>Facility:</strong> {facilityName}</li>
                        <li><strong>Appointment ID:</strong> {appointment.Id}</li>
                    </ul>
                    <p><em>Status: ⏳ Pending Confirmation</em></p>
                    <p>You will receive another email once the doctor confirms your appointment.</p>
                    <p>Thank you for choosing our healthcare services!</p>";

                await _emailSender.SendEmailAsync(patientEmail, subject, body);
                _logger.LogInformation("✅ Appointment creation email sent to {Email}", patientEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to send appointment creation email");
                throw;
            }
        }

        public async Task SendAppointmentConfirmedEmailAsync(Appointment appointment)
        {
            try
            {
                var patientEmail = appointment.Patient?.User?.Email ?? "default@email.com";
                var patientName = appointment.Patient?.FirstName ?? "Patient";
                var doctorName = $"{appointment.Doctor?.FirstName} {appointment.Doctor?.LastName}".Trim();
                var facilityName = appointment.Facility?.Name ?? "Medical Facility";
                var formattedTime = FormatTimeSpan(appointment.ScheduledTime);

                var subject = "✅ Your Appointment Has Been Confirmed";
                var body = $@"
                    <h2>Appointment Confirmed! 🎉</h2>
                    <p>Dear {patientName},</p>
                    <p>Great news! Dr. {doctorName} has confirmed your appointment.</p>
                    <p><strong>Confirmed Appointment Details:</strong></p>
                    <ul>
                        <li><strong>Doctor:</strong> Dr. {doctorName}</li>
                        <li><strong>Date:</strong> {appointment.ScheduledDate:dddd, MMMM dd, yyyy}</li>
                        <li><strong>Time:</strong> {formattedTime}</li>
                        <li><strong>Duration:</strong> {appointment.DurationMinutes} minutes</li>
                        <li><strong>Facility:</strong> {facilityName}</li>
                    </ul>
                    <p><em>Status: ✅ Confirmed</em></p>
                    <p><strong>Important Reminders:</strong></p>
                    <ul>
                        <li>Please arrive 15 minutes before your scheduled time</li>
                        <li>Bring your ID and insurance card</li>
                        <li>Cancel at least 24 hours in advance if you cannot make it</li>
                    </ul>
                    <p>We look forward to seeing you!</p>";

                await _emailSender.SendEmailAsync(patientEmail, subject, body);
                _logger.LogInformation("✅ Appointment confirmation email sent to {Email}", patientEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to send appointment confirmation email");
                throw;
            }
        }

        public async Task SendAppointmentCompletedEmailAsync(Appointment appointment)
        {
            try
            {
                var patientEmail = appointment.Patient?.User?.Email ?? "default@email.com";
                var patientName = appointment.Patient?.FirstName ?? "Patient";
                var doctorName = $"{appointment.Doctor?.FirstName} {appointment.Doctor?.LastName}".Trim();
                var formattedTime = FormatTimeSpan(appointment.ScheduledTime);

                var subject = "📋 Your Appointment Has Been Completed";
                var body = $@"
                    <h2>Appointment Completed</h2>
                    <p>Dear {patientName},</p>
                    <p>Your appointment with Dr. {doctorName} has been marked as completed.</p>
                    <p><strong>Appointment Summary:</strong></p>
                    <ul>
                        <li><strong>Doctor:</strong> Dr. {doctorName}</li>
                        <li><strong>Date:</strong> {appointment.ScheduledDate:dddd, MMMM dd, yyyy}</li>
                        <li><strong>Time:</strong> {formattedTime}</li>
                    </ul>
                    <p><em>Status: 🏁 Completed</em></p>
                    <p>Your medical records and any prescriptions have been updated in your patient portal.</p>
                    <p>Thank you for trusting us with your healthcare needs!</p>";

                await _emailSender.SendEmailAsync(patientEmail, subject, body);
                _logger.LogInformation("✅ Appointment completion email sent to {Email}", patientEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to send appointment completion email");
                throw;
            }
        }

        public async Task SendAppointmentCancelledEmailAsync(Appointment appointment, string cancellationReason)
        {
            try
            {
                var patientEmail = appointment.Patient?.User?.Email ?? "default@email.com";
                var patientName = appointment.Patient?.FirstName ?? "Patient";
                var doctorName = $"{appointment.Doctor?.FirstName} {appointment.Doctor?.LastName}".Trim();
                var formattedTime = FormatTimeSpan(appointment.ScheduledTime);

                var subject = "❌ Your Appointment Has Been Cancelled";
                var body = $@"
                    <h2>Appointment Cancelled</h2>
                    <p>Dear {patientName},</p>
                    <p>Your appointment with Dr. {doctorName} has been cancelled.</p>
                    <p><strong>Cancelled Appointment Details:</strong></p>
                    <ul>
                        <li><strong>Doctor:</strong> Dr. {doctorName}</li>
                        <li><strong>Date:</strong> {appointment.ScheduledDate:dddd, MMMM dd, yyyy}</li>
                        <li><strong>Time:</strong> {formattedTime}</li>
                        <li><strong>Reason:</strong> {cancellationReason}</li>
                    </ul>
                    <p><em>Status: ❌ Cancelled</em></p>
                    <p>If you need to reschedule, please contact our office or book a new appointment through our portal.</p>
                    <p>We hope to see you again soon!</p>";

                await _emailSender.SendEmailAsync(patientEmail, subject, body);
                _logger.LogInformation("✅ Appointment cancellation email sent to {Email}", patientEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to send appointment cancellation email");
                throw;
            }
        }

        /// <summary>
        /// Formats a TimeSpan to a readable time string (e.g., "2:30 PM")
        /// </summary>
        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            try
            {
                // Create a DateTime with today's date and the time from TimeSpan
                var dateTime = DateTime.Today.Add(timeSpan);
                
                // Format as 12-hour clock with AM/PM
                return dateTime.ToString("h:mm tt");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to format TimeSpan: {TimeSpan}, using fallback format", timeSpan);
                // Fallback: format as simple hours and minutes
                return $"{(int)timeSpan.TotalHours}:{timeSpan.Minutes:00}";
            }
        }
    }
}