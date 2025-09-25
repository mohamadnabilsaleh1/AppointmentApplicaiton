using AppointmentApplication.Domain.Shared.Results;

using MechanicShop.Domain.Common.Results;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility
{
    public static class ApplicationHealthCareFacilityErrors
    {
        public static Error FacilityAlreadyExists(string userId) =>
            Error.Conflict(
                "HealthCareFacility.AlreadyExists",
                $"Healthcare facility already exists for user ID: {userId}");

        public static Error InvalidAddress(string details) =>
            Error.Validation(
                "HealthCareFacility.InvalidAddress",
                $"Invalid address provided: {details}");

        public static Error InvalidFacilityCreation(string details) =>
            Error.Validation(
                "HealthCareFacility.InvalidCreation",
                $"Failed to create healthcare facility: {details}");

        public static Error DatabaseSaveFailed(string errorMessage) =>
            Error.Failure(
                "HealthCareFacility.DatabaseSaveFailed",
                $"Failed to save healthcare facility to database: {errorMessage}");

        public static Error InvalidCoordinates =>
            Error.Validation(
                "HealthCareFacility.InvalidCoordinates",
                "GPS coordinates are invalid");

        public static Error FacilityNameAlreadyExists(string name) =>
            Error.Conflict(
                "HealthCareFacility.Name.AlreadyExists",
                $"Healthcare facility with name '{name}' already exists");

        public static Error FacilityNotFound(Guid facilityId) =>
Error.NotFound(
   "HealthCareFacility.NotFound",
   $"Healthcare facility with ID '{facilityId}' was not found.");

        public static Error FacilityInactive(Guid facilityId) =>
            Error.Conflict(
                "HealthCareFacility.Inactive",
                $"Healthcare facility with ID '{facilityId}' is inactive and cannot be modified.");

        public static Error UpdateNotAllowed(string reason) =>
            Error.Validation(
                "HealthCareFacility.Update.NotAllowed",
                $"Update not allowed: {reason}");
    }
}