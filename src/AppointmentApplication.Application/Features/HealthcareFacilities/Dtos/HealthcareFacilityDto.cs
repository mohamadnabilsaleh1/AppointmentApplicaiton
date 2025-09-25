using System;
using System.Collections.Generic;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;

public record HealthcareFacilityDto(
    Guid Id,
    string UserId,
    string Name,
    HealthCareType Type,
    AddressDto Address,
    double GPSLatitude,
    double GPSLongitude,
    bool IsActive,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? LastModifiedAt,
    string? LastModifiedBy,
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

public record ScheduleDto(
    Guid Id,
    Guid HealthcareFacilityId,
    DayOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string Status,
    bool IsAvailable,
    string Note
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
