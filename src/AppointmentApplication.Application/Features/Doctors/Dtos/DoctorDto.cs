using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Doctors.Enums;

using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.Application.Features.Doctors.Dtos
{
    public record DoctorDto(
        Guid Id,
        Guid HealthCareFacilityId,
        string FirstName,
        string LastName,
        Gender Gender,
        Specialization Specialization,
        string Description,
        int Age
    );
}
