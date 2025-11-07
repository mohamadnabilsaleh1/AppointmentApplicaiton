// AppointmentApplication.Domain/Shared/Errors/EmailErrors.cs
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Shared.Errors
{
    public static class EmailErrors
    {
        public static readonly Error NotFound =
            Error.NotFound("Email.NotFound", "Email not found.");

        public static readonly Error InvalidUserId =
            Error.Validation("Email.InvalidUserId", "User ID cannot be empty.");

        public static readonly Error InvalidEmailAddress =
            Error.Validation("Email.InvalidEmailAddress", "Email address format is invalid.");

        public static readonly Error EmptyEmailAddress =
            Error.Validation("Email.EmptyEmailAddress", "Email address cannot be empty.");

        public static readonly Error EmptyLabel =
            Error.Validation("Email.EmptyLabel", "Email label cannot be empty.");

        // public static readonly EmailAlreadyExists =
        //     Error.Conflict("Email.AlreadyExists", "Email address already exists for another user.");
        public static readonly Error EmailAlreadyExists =
            Error.Conflict("Email.AlreadyExists", "Email address already exists for another user.");

        public static readonly Error DuplicatePrimary =
            Error.Conflict("Email.DuplicatePrimary", "User already has a primary email.");

        public static readonly Error LabelTooLong =
            Error.Validation("Email.LabelTooLong", "Email label cannot exceed 50 characters.");

        public static readonly Error EmailAddressTooLong =
            Error.Validation("Email.EmailAddressTooLong", "Email address cannot exceed 255 characters.");

        public static readonly Error CannotRemovePrimary =
            Error.Conflict("Email.CannotRemovePrimary", "Cannot remove primary email. Set another email as primary first.");

        public static readonly Error CannotUpdatePrimary =
            Error.Conflict("Email.CannotUpdatePrimary", "Cannot update primary status directly. Use SetPrimaryEmail method.");
    }
}