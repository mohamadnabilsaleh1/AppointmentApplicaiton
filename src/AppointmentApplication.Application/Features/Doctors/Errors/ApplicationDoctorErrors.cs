using System;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Application.Features.Doctors.Errors
{
    public static class ApplicationDoctorErrors
    {
        // 🧩 General Errors
        public static Error DatabaseSaveFailed(string message) =>
            Error.Failure("ApplicationDoctor.DatabaseSaveFailed", message);

        public static readonly Error UnexpectedError =
            Error.Failure("ApplicationDoctor.UnexpectedError", "An unexpected error occurred while processing the doctor request.");


        // 🧩 User-Related Errors
        public static Error UserAlreadyExists(string email) =>
            Error.Conflict("ApplicationDoctor.UserAlreadyExists", $"A user with the email '{email}' already exists.");

        public static readonly Error UserCreationFailed =
            Error.Failure("ApplicationDoctor.UserCreationFailed", "Failed to create user account for the doctor.");

        public static Error FacilityNotActive(string facilityName) =>
            Error.Validation("ApplicationDoctor.FacilityNotActive", $"The healthcare facility '{facilityName}' is inactive and cannot add new doctors.");


        // 🧩 Doctor-Related Errors
        public static Error DoctorNotFound(Guid doctorId) =>
            Error.NotFound(
                "Doctor.NotFound",
                $"Doctor with ID '{doctorId}' was not found.");

        public static Error DuplicateLicenseNumber(string licenseNumber) =>
            Error.Conflict("ApplicationDoctor.DuplicateLicenseNumber", $"A doctor with license number '{licenseNumber}' already exists.");

        public static readonly Error InvalidDoctorData =
            Error.Validation("ApplicationDoctor.InvalidDoctorData", "Doctor data is invalid or incomplete.");
    }
}
