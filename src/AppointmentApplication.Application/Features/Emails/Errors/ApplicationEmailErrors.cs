using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Application.Features.Emails.Errors
{
    public class ApplicationEmailErrors
    {
        public static Error UserNotFound(Guid userId) =>
          Error.NotFound(
              "Email.UserNotFound",
              $"User with ID '{userId}' was not found.");

        public static Error EmailAlreadyExists(string emailAddress) =>
            Error.Conflict(
                "Email.AlreadyExists",
                $"Email address '{emailAddress}' already exists for this user.");

        public static Error EmailNotFound(Guid emailId) =>
            Error.NotFound(
                "Email.NotFound",
                $"Email with ID '{emailId}' was not found.");

        public static Error EmailAddressNotFound(Guid emailAddress) =>
            Error.NotFound(
                "Email.AddressNotFound",
                $"Email address '{emailAddress}' was not found.");

        public static Error AddEmailFailed(string details) =>
            Error.Failure(
                "Email.AddFailed",
                $"Failed to add email: {details}");

        public static Error UpdateEmailFailed(string details) =>
            Error.Failure(
                "Email.UpdateFailed",
                $"Failed to update email: {details}");

        public static Error DatabaseSaveFailed(string errorMessage) =>
            Error.Failure(
                "Email.DatabaseSaveFailed",
                $"Failed to save email to database: {errorMessage}");

        public static Error InvalidEmailOperation(string reason) =>
            Error.Validation(
                "Email.InvalidOperation",
                $"Invalid email operation: {reason}");
    }
}