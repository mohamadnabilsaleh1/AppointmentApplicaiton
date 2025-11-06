using System;
using System.Threading.Tasks;
using AppointmentApplication.Application.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AppointmentApplication.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                var emailSettings = _configuration.GetSection("EmailSettings");

                string host = emailSettings["Host"] ?? "mail.motorexexpo.com";
                int port = 465;
                string username = emailSettings["Username"] ?? "info@motorexexpo.com";
                string password = emailSettings["Password"] ?? "rdC#BO&qvP=$Njal";
                string from = emailSettings["From"] ?? "info@motorexexpo.com";

                _logger.LogInformation("🚀 Attempting to send email to {To} via {Host}:{Port} using SSL", to, host, port);
                _logger.LogInformation("📧 From: {From}, Subject: {Subject}", from, subject);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Shefa ", from));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;
                message.Date = DateTimeOffset.Now;

                var bodyBuilder = new BodyBuilder();
                if (isHtml)
                {
                    bodyBuilder.HtmlBody = body;
                    _logger.LogInformation("📝 HTML body length: {Length} characters", body.Length);
                }
                else
                {
                    bodyBuilder.TextBody = body;
                    _logger.LogInformation("📝 Plain text body length: {Length} characters", body.Length);
                }
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new MailKit.Net.Smtp.SmtpClient();
                client.Timeout = 30000;

                // Enable detailed logging
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                _logger.LogInformation("🔌 Connecting to {Host}:{Port} with SSL...", host, port);
                await client.ConnectAsync(host, port, SecureSocketOptions.SslOnConnect);

                _logger.LogInformation("✅ Connected successfully, authenticating...");
                await client.AuthenticateAsync(username, password);

                _logger.LogInformation("✅ Authenticated successfully, sending email...");

                // Get the SMTP server response
                var response = await client.SendAsync(message);
                _logger.LogInformation("📨 SMTP Server Response: {Response}", response);

                _logger.LogInformation("✅ Email sent successfully, disconnecting...");
                await client.DisconnectAsync(true);

                _logger.LogInformation("🎉 Email sent successfully to {To}", to);
                _logger.LogInformation("✅ Message ID: {MessageId}", message.MessageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to send email to {To}", to);
                throw;
            }
        }
    }
}