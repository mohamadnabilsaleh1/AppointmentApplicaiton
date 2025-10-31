using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Doctors.DoctorsTreatmentCapabilities;

namespace AppointmentApplication.Domain.Doctors.DoctorsTreatmentCapabilities
{
    public static class DoctorTreatmentCapacityErrors
    {
        public static readonly Error DoctorIdRequired =
            Error.Validation("Doctor.DoctorIdRequired", "Doctor ID is required");

        public static readonly Error InvalidMaxPatients =
            Error.Validation("DoctorTreatmentCapacity.InvalidMaxPatients", "Maximum patients per day must be greater than zero.");

        public static readonly Error InvalidSessionDuration =
            Error.Validation("DoctorTreatmentCapacity.InvalidSessionDuration", "Session duration must be between 1 and 1440 minutes.");

        public static readonly Error CapacityNotFound =
            Error.Validation("DoctorTreatmentCapacity.CapacityNotFound", "Doctor treatment capacity not found.");

        public static readonly Error CapacityInactive =
            Error.Validation("DoctorTreatmentCapacity.CapacityInactive", "Doctor treatment capacity is inactive.");
        public static readonly Error DoctorTreatmentCapacityNotFound = Error.Validation(
                    "DoctorTreatmentCapacity.NotFound",
                    "Doctor treatment capacity not found");
    }

}