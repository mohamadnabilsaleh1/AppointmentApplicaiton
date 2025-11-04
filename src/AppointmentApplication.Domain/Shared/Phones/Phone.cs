// AppointmentApplication.Domain/Shared/Phone.cs
using System;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Errors;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Shared
{
    public class Phone : AuditableEntity
    {
        public OwnerType OwnerType { get; private set; }
        public Guid OwnerId { get; private set; }
        public string PhoneNumber { get; private set; } = string.Empty;
        public string Label { get; private set; } = string.Empty;

        private Phone() { }

        public static Result<Phone> Create(OwnerType ownerType, Guid ownerId, string phoneNumber, string label)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return ContactErrors.EmptyPhoneNumber;
            }

            if (string.IsNullOrWhiteSpace(label))
            {
                return ContactErrors.EmptyLabel;
            }

            if (!Enum.IsDefined(typeof(OwnerType), ownerType))
            {
                return ContactErrors.InvalidOwnerType;
            }

            // Basic phone validation (customize as needed)
            if (!IsValidPhoneNumber(phoneNumber))
            {
                return ContactErrors.InvalidPhoneFormat;
            }

            var phone = new Phone
            {
                Id = Guid.NewGuid(),
                OwnerType = ownerType,
                OwnerId = ownerId,
                PhoneNumber = phoneNumber.Trim(),
                Label = label.Trim()
            };

            return phone;
        }

        public Result<Updated> Update(string phoneNumber, string label)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return ContactErrors.EmptyPhoneNumber;
            }

            if (string.IsNullOrWhiteSpace(label))
            {
                return ContactErrors.EmptyLabel;
            }

            if (!IsValidPhoneNumber(phoneNumber))
            {
                return ContactErrors.InvalidPhoneFormat;
            }

            PhoneNumber = phoneNumber.Trim();
            Label = label.Trim();
            UpdatedAtUtc = DateTime.UtcNow;

            return Result.Updated;
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            // Basic phone validation - customize based on your requirements
            return !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.Length >= 10;
        }
    }
}