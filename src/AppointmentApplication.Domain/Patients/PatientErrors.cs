using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Patients
{
    public class PatientErrors
    {
        // AllergyAlreadyExists
        public static readonly Error PatientNotFound =
        Error.NotFound("Patient.NotFound", "Patient not found.");
        public static readonly Error InvalidAllergyType =
        Error.Validation("Patient.InvalidAllergyType", "Invalid allergy type.");
        public static readonly Error UploadAlreadyExists =
        Error.NotFound("Patient.NotFound", "Patient not found.");
        public static readonly Error AllergyNotFound =
        Error.NotFound("Allergy.NotFound", "Allergy not found for the patient.");
        public static readonly Error ChronicDiseaseNotFound =
        Error.NotFound("ChronicDisease.NotFound", "Chronic disease not found for the patient.");
        public static readonly Error AllergyAlreadyExists =
        Error.NotFound("Allergy.AlreadyExists", "Allergy already exists for the patient.");
        public static readonly Error ChronicDiseaseAlreadyExists =
        Error.NotFound("ChronicDisease.AlreadyExists", "Chronic disease already exists for the patient.");
        public static readonly Error UploadNotFound =
        Error.NotFound("Upload.NotFound", "Upload not found for the patient."); 
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