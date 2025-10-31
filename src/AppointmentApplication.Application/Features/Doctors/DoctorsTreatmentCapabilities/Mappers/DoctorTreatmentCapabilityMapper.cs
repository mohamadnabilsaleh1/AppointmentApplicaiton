using System;
using System.Collections.Generic;
using System.Linq;
using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Dtos;
using AppointmentApplication.Domain.Doctors.DoctorsTreatmentCapabilities;

namespace AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Mappers
{
    public static class DoctorTreatmentCapabilityMapper
    {
        public static DoctorTreatmentCapabilityDto ToDto(this DoctorTreatmentCapacity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new DoctorTreatmentCapabilityDto(
                entity.DoctorId,
                entity.MaxPatientsPerDay,
                entity.SessionDurationMinutes,
                entity.IsActive);
        }

        public static List<DoctorTreatmentCapabilityDto> ToDtos(this IEnumerable<DoctorTreatmentCapacity> entities)
        {
            return entities?.Select(e => e.ToDto()).ToList() ?? new List<DoctorTreatmentCapabilityDto>();
        }
   
    }
}