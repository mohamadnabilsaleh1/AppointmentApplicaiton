
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;
using AppointmentApplication.Domain.Doctors;

namespace AppointmentApplication.Application.Features.Doctors.Mappers
{
    public static class DoctorMapper
    {
        public static DoctorDto ToDto(this Doctor entity)
        {
            int age = Doctor.CalculateAge(entity.DateOfBirth);
            return new DoctorDto(entity.Id, entity.FacilityId, entity.FirstName, entity.LastName, entity.Gender, entity.Specialization, age);
        }

        public static List<DoctorDto> ToDtos(this IEnumerable<Doctor> entities)
        {
            return entities.Select(e => e.ToDto()).ToList();
        }
    }
}

