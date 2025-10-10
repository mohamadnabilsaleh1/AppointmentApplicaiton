using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Application.Features.Patients.Errors
{
    public static class ApplicationPatientErrors
    {
        public static Error PatientNotFound(Guid patientId) =>
            Error.NotFound(
                "Patient.NotFound",
                $"Patient with ID '{patientId}' was not found.");
        public static Error AllergyNotFound(Guid patientId) =>
            Error.NotFound(
                "Allergy.NotFound",
                $"Allergy with ID '{patientId}' was not found for the patient.");
    }
}