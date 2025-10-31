using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Dtos
{
    public record DoctorTreatmentCapabilityDto(Guid DoctorId, int MaxPatientsPerDay,
    int SessionDurationMinutes, bool IsActive);   
}