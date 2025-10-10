using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Application.Features.Patients.Errors
{
    public static class ApplicationPatientErrors
    {
        public static Error PatientNotFound(Guid facilityId) =>
            Error.NotFound(
                "Patient.NotFound",
                $"Patient with ID '{facilityId}' was not found.");
    }
}