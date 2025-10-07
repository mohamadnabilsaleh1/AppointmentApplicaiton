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
        int Age
    );
}

/*
  Guid UserId,
        string FirstName,
        string LastName,
        string Email,
        string Password,
        Gender Gender,
        string LicenseNumber,
        Specialization Specialization,
        DateOnly DateOfBirth
*/

/*
public record ScheduleDto(
    Guid Id,
    DaysOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string Note
);
*/