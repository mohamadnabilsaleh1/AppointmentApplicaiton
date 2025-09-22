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
}
