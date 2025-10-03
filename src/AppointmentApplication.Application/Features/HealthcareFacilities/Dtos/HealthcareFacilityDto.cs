using System;
using System.Collections.Generic;

using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;

using AppointmentApplication.Application.Features.Users.Dtos;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.Users;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;

public record HealthcareFacilityDto(
    Guid Id,
    string Name,
    HealthCareType Type,
    AddressDto Address,
    double GPSLatitude,
    double GPSLongitude,
    IReadOnlyCollection<DepartmentDto> Departments,
    IReadOnlyCollection<ScheduleDto> Schedules,
    IReadOnlyCollection<ScheduleExceptionDto> ScheduleExceptions
);
public record HealthcareFacilityWithUserDto(
    Guid Id,
    string Name,
    HealthCareType Type,
    AddressDto Address,
    double GPSLatitude,
    double GPSLongitude,
    UserDto User,
    IReadOnlyCollection<DepartmentDto> Departments,
    IReadOnlyCollection<ScheduleDto> Schedules,
    IReadOnlyCollection<ScheduleExceptionDto> ScheduleExceptions
);

public record AddressDto(
    string Street,
    string City,
    string Country,
    string ZipCode
)
{
    public string FullAddress => $"{Street}, {City}, {Country} {ZipCode}";
}

public record DepartmentDto(
    Guid Id,
    Guid HealthcareFacilityId,
    string Name,
    string Description,
    bool IsActive,
    DateTime CreatedAt
);



public record ScheduleExceptionDto(
    Guid Id,
    Guid HealthcareFacilityId,
    DateOnly Date,
    DayOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string Status,
    string Reason
);
