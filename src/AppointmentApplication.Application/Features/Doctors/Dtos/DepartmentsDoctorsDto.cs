using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Doctors.Enums;

using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.Application.Features.Doctors.Dtos
{

    public record DepartmentDoctorsDto(
Guid Id,
Guid HealthCareFacilityId,
string FirstName,
string LastName,
Gender Gender,
Specialization Specialization,
int age,
string Description,
double AverageRating,
int TotalReviews
);

}
