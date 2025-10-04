using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Users
{
    public static class UserErrors
    {
        public static readonly Error FirstNameRequired =
            Error.Validation(
                "User.FirstNameRequired",
                "First name is required.");

        public static readonly Error LastNameRequired =
            Error.Validation(
                "User.LastNameRequired",
                "Last name is required.");

        public static readonly Error EmailRequired =
            Error.Validation(
                "User.EmailRequired",
                "Email is required.");

        public static readonly Error InvalidEmail =
            Error.Validation(
                "User.InvalidEmail",
                "The provided email address is not valid.");

        public static readonly Error RoleRequired =
            Error.Validation(
                "User.RoleRequired",
                "At least one role must be assigned to the user.");
    }
}
