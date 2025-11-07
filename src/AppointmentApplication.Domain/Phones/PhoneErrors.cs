// AppointmentApplication.Domain/Shared/Errors/PhoneErrors.cs
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Shared.Errors
{
    public static class PhoneErrors
    {
        public static readonly Error NotFound =
            Error.NotFound("Phone.NotFound", "Phone not found.");

        public static readonly Error InvalidUserId =
            Error.Validation("Phone.InvalidUserId", "User ID cannot be empty.");

        public static readonly Error InvalidPhoneNumber =
            Error.Validation("Phone.InvalidPhoneNumber", "Phone number format is invalid.");

        public static readonly Error EmptyPhoneNumber =
            Error.Validation("Phone.EmptyPhoneNumber", "Phone number cannot be empty.");

        public static readonly Error EmptyLabel =
            Error.Validation("Phone.EmptyLabel", "Phone label cannot be empty.");

        public static readonly Error PhoneAlreadyExists =
            Error.Conflict("Phone.AlreadyExists", "Phone number already exists for another user.");

        public static readonly Error DuplicatePrimary =
            Error.Conflict("Phone.DuplicatePrimary", "User already has a primary phone.");

        public static readonly Error LabelTooLong =
            Error.Validation("Phone.LabelTooLong", "Phone label cannot exceed 50 characters.");

        public static readonly Error PhoneNumberTooLong =
            Error.Validation("Phone.PhoneNumberTooLong", "Phone number cannot exceed 20 characters.");

        public static readonly Error InvalidCountryCode =
            Error.Validation("Phone.InvalidCountryCode", "Country code is invalid.");

        public static readonly Error CannotRemovePrimary =
            Error.Conflict("Phone.CannotRemovePrimary", "Cannot remove primary phone. Set another phone as primary first.");

        public static readonly Error CannotUpdatePrimary =
            Error.Conflict("Phone.CannotUpdatePrimary", "Cannot update primary status directly. Use SetPrimaryPhone method.");
    }
}