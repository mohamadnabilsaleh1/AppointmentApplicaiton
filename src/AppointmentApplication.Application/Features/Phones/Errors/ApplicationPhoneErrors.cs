using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Application.Features.Phones.Errors
{
    public class ApplicationPhoneErrors
    {
        public static Error UserNotFound(Guid userId) =>
                  Error.NotFound(
                      "Phone.UserNotFound",
                      $"User with ID '{userId}' was not found.");

        public static Error PhoneAlreadyExists(string phoneNumber) =>
            Error.Conflict(
                "Phone.AlreadyExists",
                $"Phone number '{phoneNumber}' already exists for this user.");

        public static Error PhoneNotFound(Guid phoneId) =>
            Error.NotFound(
                "Phone.NotFound",
                $"Phone with ID '{phoneId}' was not found.");

        public static Error PhoneNumberNotFound(Guid phoneNumber) =>
            Error.NotFound(
                "Phone.NumberNotFound",
                $"Phone number '{phoneNumber}' was not found.");

        public static Error AddPhoneFailed(string details) =>
            Error.Failure(
                "Phone.AddFailed",
                $"Failed to add phone: {details}");

        public static Error UpdatePhoneFailed(string details) =>
            Error.Failure(
                "Phone.UpdateFailed",
                $"Failed to update phone: {details}");

        public static Error DatabaseSaveFailed(string errorMessage) =>
            Error.Failure(
                "Phone.DatabaseSaveFailed",
                $"Failed to save phone to database: {errorMessage}");

        public static Error InvalidPhoneOperation(string reason) =>
            Error.Validation(
                "Phone.InvalidOperation",
                $"Invalid phone operation: {reason}");
        public static Error MultiplePrimaryPhones(Guid userId) => Error.Validation(
      "Phone.MultiplePrimaryPhones",
      $"User {userId} cannot have multiple primary phones");
    }
}
