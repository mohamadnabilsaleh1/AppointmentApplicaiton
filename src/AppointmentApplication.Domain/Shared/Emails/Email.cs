// AppointmentApplication.Domain/Shared/Email.cs
using System;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Errors;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Shared
{
    public class Email : AuditableEntity
    {
        public OwnerType OwnerType { get; private set; }
        public Guid OwnerId { get; private set; }
        public string EmailAddress { get; private set; } = string.Empty;
        public string Label { get; private set; } = string.Empty;

        private Email() { }

        public static Result<Email> Create(OwnerType ownerType, Guid ownerId, string emailAddress, string label)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return ContactErrors.EmptyEmailAddress;
            }

            if (!IsValidEmail(emailAddress))
            {
                return ContactErrors.InvalidEmailFormat;
            }

            if (string.IsNullOrWhiteSpace(label))
            {
                return ContactErrors.EmptyLabel;
            }

            if (!Enum.IsDefined(typeof(OwnerType), ownerType))
            {
                return ContactErrors.InvalidOwnerType;
            }

            var email = new Email
            {
                Id = Guid.NewGuid(),
                OwnerType = ownerType,
                OwnerId = ownerId,
                EmailAddress = emailAddress.Trim().ToLower(),
                Label = label.Trim()
            };

            return email;
        }

        public Result<Updated> Update(string emailAddress, string label)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return ContactErrors.EmptyEmailAddress;
            }

            if (!IsValidEmail(emailAddress))
            {
                return ContactErrors.InvalidEmailFormat;
            }

            if (string.IsNullOrWhiteSpace(label))
            {
                return ContactErrors.EmptyLabel;
            }

            EmailAddress = emailAddress.Trim().ToLower();
            Label = label.Trim();
            UpdatedAtUtc = DateTime.UtcNow;

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