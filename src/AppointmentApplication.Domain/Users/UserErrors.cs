using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Users;
    public static class UserErrors
    {
        public static readonly Error NotFound = Error.NotFound(
            "User.NotFound",
            "The user with the specified identifier was not found");

        public static readonly Error InvalidCredentials = Error.Validation(
            "User.InvalidCredentials",
            "The provided credentials were invalid");
    }
