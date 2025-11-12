using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

namespace AppointmentApplication.Application.Features.Emails.Commands.UpdateEmail
{
    public class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
    {
        public UpdateEmailCommandValidator()
        {
            // UserId validation
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required")
                .NotEqual(Guid.Empty).WithMessage("User ID cannot be empty");

            // Email address validation
            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("Email address is required")
                .MaximumLength(255).WithMessage("Email address cannot exceed 255 characters")
                .EmailAddress().WithMessage("Invalid email address format")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Invalid email address format")
                .Must(NotContainConsecutiveDots).WithMessage("Email address cannot contain consecutive dots")
                .Must(HaveValidDomain).WithMessage("Email domain is invalid")
                .Must(NotStartOrEndWithDot).WithMessage("Email address cannot start or end with a dot");

            // Label validation
            RuleFor(x => x.Label)
                .NotEmpty().WithMessage("Email label is required")
                .MaximumLength(50).WithMessage("Email label cannot exceed 50 characters")
                .MinimumLength(2).WithMessage("Email label must be at least 2 characters long")
                .Matches(@"^[a-zA-Z0-9\s\-_]+$").WithMessage("Email label contains invalid characters")
                .Must(NotContainOnlyNumbers).WithMessage("Email label cannot contain only numbers")
                .Must(NotContainConsecutiveSpaces).WithMessage("Email label cannot contain consecutive spaces");

            // IsPrimary validation (no specific rules needed, but can add custom logic if required)
            RuleFor(x => x.IsPrimary)
                .NotNull().WithMessage("IsPrimary flag is required");

            // Cross-property validation
            RuleFor(x => x)
                .Must(HaveValidEmailLength).WithMessage("Email address is too long after normalization")
                .Must(NotBeDisposableEmail).WithMessage("Disposable email addresses are not allowed");
        }

        private bool NotContainConsecutiveDots(string email)
        {
            return !string.IsNullOrEmpty(email) && !email.Contains("..");
        }

        private bool NotStartOrEndWithDot(string email)
        {
            return !string.IsNullOrEmpty(email) &&
                   !email.StartsWith('.') &&
                   !email.EndsWith('.');
        }

        private bool HaveValidDomain(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            var parts = email.Split('@');
            if (parts.Length != 2)
            {

                return false;
            }

            var domain = parts[1];
            return domain.Contains('.') &&
                   domain.Length >= 3 &&
                   !domain.StartsWith('.') &&
                   !domain.EndsWith('.');
        }

        private bool NotContainOnlyNumbers(string label)
        {
            return !string.IsNullOrEmpty(label) && !label.All(char.IsDigit);
        }

        private bool NotContainConsecutiveSpaces(string label)
        {
            return !string.IsNullOrEmpty(label) && !label.Contains("  ");
        }

        private bool HaveValidEmailLength(UpdateEmailCommand command)
        {
            if (string.IsNullOrEmpty(command.EmailAddress))
            {

                return false;
            }

            var normalizedEmail = command.EmailAddress.Trim().ToLowerInvariant();
            return normalizedEmail.Length <= 255;
        }

        private bool NotBeDisposableEmail(UpdateEmailCommand command)
        {
            if (string.IsNullOrEmpty(command.EmailAddress))
            {

                return false;
            }

            var disposableDomains = new[]
            {
                "tempmail.com", "throwaway.com", "fakeinbox.com", "guerrillamail.com",
                "mailinator.com", "10minutemail.com", "temp-mail.org", "yopmail.com",
                "getairmail.com", "sharklasers.com", "grr.la", "spam4.me"
            };

            var email = command.EmailAddress.ToLowerInvariant();
            return !disposableDomains.Any(domain => email.EndsWith("@" + domain));
        }
    }
}
