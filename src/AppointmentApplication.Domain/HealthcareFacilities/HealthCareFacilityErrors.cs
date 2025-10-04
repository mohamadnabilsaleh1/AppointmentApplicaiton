using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.HealthcareFacilities;

public static class HealthCareFacilityErrors
{
    public static readonly Error UserIdRequired =
        Error.Validation("HealthCareFacility.UserId.Required", "User ID is required.");

    public static readonly Error NameRequired =
        Error.Validation("HealthCareFacility.Name.Required", "Name is required.");

    public static readonly Error AddressRequired =
        Error.Validation("HealthCareFacility.Address.Required", "Address is required.");
    public static readonly Error InvalidCoordinates =
        Error.Validation(
            "HealthcareFacility.InvalidCoordinates",
            "The provided GPS coordinates are invalid. Latitude must be between -90 and 90, and Longitude must be between -180 and 180.");
}
