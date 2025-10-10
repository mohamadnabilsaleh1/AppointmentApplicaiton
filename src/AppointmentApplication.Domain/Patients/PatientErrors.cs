using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Patients
{
    public class PatientErrors
    {
        public static readonly Error PatientNotFound =
        Error.NotFound("Patient.NotFound", "Patient not found.");

        public static readonly Error InvalidName =
            Error.Validation("Patient.InvalidName", "First name and last name cannot be empty.");

        public static readonly Error NationalId =
            Error.Validation("Patient.NationalID", "License number cannot be empty.");

        public static readonly Error InvalidDateOfBirth =
            Error.Validation("Patient.InvalidDateOfBirth", "Date of birth must be in the past.");

        public static readonly Error InvalidGender =
            Error.Validation("Patient.InvalidGender", "Gender is required.");
    }
}