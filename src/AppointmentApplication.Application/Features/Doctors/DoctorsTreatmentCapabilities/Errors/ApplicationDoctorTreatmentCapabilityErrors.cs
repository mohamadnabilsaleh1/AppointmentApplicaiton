using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Errors
{
    public class ApplicationDoctorTreatmentCapabilityErrors
    {
        public static Error DoctorTreatmentCapabilityNotFound(Guid doctorId) =>
            Error.NotFound(
                "Doctor.TreatmentCapability.NotFound",
                $"Treatment Capability for Doctor with ID '{doctorId}' was not found.");
        public static Error DoctorTreatmentCapabilityAlreadyExist(Guid doctorId) =>
    Error.NotFound(
        "Doctor.TreatmentCapability.AlreadyExist",
        $"Treatment Capability for Doctor with ID '{doctorId}' already exist");
    }
}