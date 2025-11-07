// AppointmentApplication.Domain/Shared/Email.cs
using System;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Shared.Errors;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;

namespace AppointmentApplication.Domain.Emails
{
    public class Email : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public string EmailAddress { get; private set; }
        public string Label { get; private set; }
        public bool IsPrimary { get; private set; }

        public User? User { get; private set; }

        // Private constructor - only accessible within this class
        private Email(
            Guid id,
            Guid userId,
            string emailAddress,
            string label,
            bool isPrimary)
        {
            Id = id;
            UserId = userId;
            EmailAddress = emailAddress;
            Label = label;
            IsPrimary = isPrimary;
            CreatedAtUtc = DateTime.UtcNow;
        }

        // Parameterless private constructor for EF Core
        private Email() { }

        public static Result<Email> Create(
            Guid userId,
            string emailAddress,
            string label,
            bool isPrimary = false)
        {
            // Validation
            if (userId == Guid.Empty)
            {
                return EmailErrors.InvalidUserId;
            }

            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return EmailErrors.EmptyEmailAddress;
            }

            if (string.IsNullOrWhiteSpace(label))
            {
                return EmailErrors.EmptyLabel;
            }

            if (emailAddress.Length > 255)
            {
                return EmailErrors.EmailAddressTooLong;
            }

            if (label.Length > 50)
            {
                return EmailErrors.LabelTooLong;
            }

            if (!IsValidEmail(emailAddress))
            {
                return EmailErrors.InvalidEmailAddress;
            }

            // Use the private constructor to create instance
            var email = new Email(
                id: Guid.NewGuid(),
                userId: userId,
                emailAddress: emailAddress.ToLowerInvariant().Trim(),
                label: label.Trim(),
                isPrimary: isPrimary
            );

            return email;
        }

        public Result<Updated> SetPrimary(bool isPrimary)
        {
            if (IsPrimary == isPrimary)
            {
                return Result.Updated; // No change needed
            }

            IsPrimary = isPrimary;
            UpdatedAtUtc = DateTime.UtcNow;

            return Result.Updated;
        }

        public Result<Updated> UpdateLabel(string newLabel, string modifiedBy)
        {
            if (string.IsNullOrWhiteSpace(newLabel))
            {
                return EmailErrors.EmptyLabel;
            }

            if (newLabel.Length > 50)
            {
                return EmailErrors.LabelTooLong;
            }

            Label = newLabel.Trim();
            UpdatedAtUtc = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;

            return Result.Updated;
        }

        public Result<Updated> UpdateEmailAddress(string newEmailAddress, string modifiedBy)
        {
            if (string.IsNullOrWhiteSpace(newEmailAddress))
            {
                return EmailErrors.EmptyEmailAddress;
            }

            if (newEmailAddress.Length > 255)
            {
                return EmailErrors.EmailAddressTooLong;
            }

            if (!IsValidEmail(newEmailAddress))
            {
                return EmailErrors.InvalidEmailAddress;
            }

            EmailAddress = newEmailAddress.ToLowerInvariant().Trim();
            UpdatedAtUtc = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;

            return Result.Updated;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}