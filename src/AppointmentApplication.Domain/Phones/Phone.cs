// AppointmentApplication.Domain/Shared/Phone.cs
using System;
using System.Linq;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Shared.Errors;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;

namespace AppointmentApplication.Domain.Phones
{
    public class Phone : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Label { get; private set; }
        public bool IsPrimary { get; private set; }

        public virtual User User { get; private set; } = null!;

        // Private constructor - only accessible within this class
        private Phone(
            Guid id,
            Guid userId,
            string phoneNumber,
            string label,
            bool isPrimary,
            string createdBy,
            DateTime createdAtUtc)
        {
            Id = id;
            UserId = userId;
            PhoneNumber = phoneNumber;
            Label = label;
            IsPrimary = isPrimary;
            CreatedBy = createdBy;
            CreatedAtUtc = createdAtUtc;
        }

        // Parameterless private constructor for EF Core
        private Phone() { }

        public static Result<Phone> Create(
            Guid userId,
            string phoneNumber,
            string label,
            bool isPrimary = false,
            string createdBy = "system")
        {
            // Validation
            if (userId == Guid.Empty)
            {
                return PhoneErrors.InvalidUserId;
            }

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return PhoneErrors.EmptyPhoneNumber;
            }

            if (string.IsNullOrWhiteSpace(label))
            {
                return PhoneErrors.EmptyLabel;
            }

            if (phoneNumber.Length > 20)
            {
                return PhoneErrors.PhoneNumberTooLong;
            }

            if (label.Length > 50)
            {
                return PhoneErrors.LabelTooLong;
            }

            if (!IsValidPhoneNumber(phoneNumber))
            {
                return PhoneErrors.InvalidPhoneNumber;
            }

            // Use the private constructor to create instance
            var phone = new Phone(
                id: Guid.NewGuid(),
                userId: userId,
                phoneNumber: phoneNumber.Trim(),
                label: label.Trim(),
                isPrimary: isPrimary,
                createdBy: createdBy,
                createdAtUtc: DateTime.UtcNow
            );

            return phone;
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
                return PhoneErrors.EmptyLabel;
            }

            if (newLabel.Length > 50)
            {
                return PhoneErrors.LabelTooLong;
            }

            Label = newLabel.Trim();
            UpdatedAtUtc = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;

            return Result.Updated;
        }

        public Result<Updated> UpdatePhoneNumber(string newPhoneNumber, string modifiedBy)
        {
            if (string.IsNullOrWhiteSpace(newPhoneNumber))
            {
                return PhoneErrors.EmptyPhoneNumber;
            }

            if (newPhoneNumber.Length > 20)
            {
                return PhoneErrors.PhoneNumberTooLong;
            }

            if (!IsValidPhoneNumber(newPhoneNumber))
            {
                return PhoneErrors.InvalidPhoneNumber;
            }

            PhoneNumber = newPhoneNumber.Trim();
            UpdatedAtUtc = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;

            return Result.Updated;
        }

        private static bool IsValidPhoneNumber(string number)
        {
            // Remove all non-digit characters and check length
            var digitsOnly = new string(number.Where(char.IsDigit).ToArray());
            return digitsOnly.Length >= 10;
        }
    }
}