using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Doctors
{
    public static class DoctorErrors
    {
        public static readonly Error DoctorNotFound =
            Error.NotFound("Doctor.NotFound", "Doctor not found.");

        public static readonly Error InvalidName =
            Error.Validation("Doctor.InvalidName", "First name and last name cannot be empty.");

        public static readonly Error InvalidLicenseNumber =
            Error.Validation("Doctor.InvalidLicenseNumber", "License number cannot be empty.");

        public static readonly Error InvalidDateOfBirth =
            Error.Validation("Doctor.InvalidDateOfBirth", "Date of birth must be in the past.");

        public static readonly Error InvalidSpecialization =
            Error.Validation("Doctor.InvalidSpecialization", "Doctor must have a valid specialization.");

        public static readonly Error InvalidGender =
            Error.Validation("Doctor.InvalidGender", "Gender is required.");
    }
}
