using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Dtos;
using AppointmentApplication.Domain.Patients;

namespace AppointmentApplication.Application.Features.Patients.Mappers
{
    public static class PatientMapper
    {
        public static PatientDto ToDto(this Patient entity)
        {
            return new PatientDto(
                entity.Id,
                entity.NationalID,
                entity.Gender,
                entity.FirstName,
                entity.LastName,
                entity.DateOfBirth
            );
        }
    }
}
