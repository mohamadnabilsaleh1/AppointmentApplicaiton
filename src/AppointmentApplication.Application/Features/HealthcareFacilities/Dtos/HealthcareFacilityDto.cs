using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;

public class HealthcareFacilityDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public HealthCareType Type { get; set; }
    public AddressDto Address { get; set; } = new();
    public double GPSLatitude { get; set; }
    public double GPSLongitude { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
    public List<DepartmentDto> Departments { get; set; } = new();
    public List<ScheduleDto> Schedules { get; set; } = new();
    public List<ScheduleExceptionDto> ScheduleExceptions { get; set; } = new();
}

public class AddressDto
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string FullAddress => $"{Street}, {City}, {Country} {ZipCode}";
}

public class DepartmentDto
{
    public Guid Id { get; set; }
    public Guid HealthcareFacilityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ScheduleDto
{
    public Guid Id { get; set; }
    public Guid HealthcareFacilityId { get; set; }
    public DaysOfWeek DayOfWeek { get; set; }
    public TimeSpan OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; }
    public bool IsActive { get; set; }
}

public class ScheduleExceptionDto
{
    public Guid Id { get; set; }
    public Guid HealthcareFacilityId { get; set; }
    public DateTime ExceptionDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public bool IsAllDay => !StartTime.HasValue && !EndTime.HasValue;
}