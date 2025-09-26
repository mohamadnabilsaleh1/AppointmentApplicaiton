using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Application.Features.Users.Errors;

public static class ApplicationUserErrors
{
    public static readonly Error InvalidCredentials = Error.Validation(
        "User.InvalidCredentials",
        "The provided email or password is incorrect.");

    public static Error UserNotFound(string email) => Error.NotFound(
        "User.NotFound",
        $"No user account found with email '{email}'.");

    public static readonly Error AccountLocked = Error.Conflict(
        "User.AccountLocked",
        "The user account is locked due to multiple failed login attempts.");

    public static readonly Error AccountInactive = Error.Conflict(
        "User.AccountInactive",
        "The user account is inactive. Please contact support.");

    public static Error DatabaseError(string details) => Error.Failure(
        "User.DatabaseError",
        $"A database error occurred while processing the login request: {details}");

    public static readonly Error AuthenticationFailed = Error.Failure(
        "Keycloak.AuthenticationFailed",
        "Failed to acquire access token do to authentication failure");
}
