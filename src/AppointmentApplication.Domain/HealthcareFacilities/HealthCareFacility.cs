using System;
using System.Collections.Generic;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.HealthcareFacilities.Departments;
using AppointmentApplication.Domain.HealthcareFacilities.Schedules;
using AppointmentApplication.Domain.HealthcareFacilities.ScheduleExceptions;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;

namespace AppointmentApplication.Domain.HealthcareFacilities;

public sealed class HealthCareFacility : AuditableEntity
{
    public string UserId { get; private set; }
    public string Name { get; private set; }
    public HealthCareType Type { get; private set; }
    public Address Address { get; private set; }
    public double GPSLatitude { get; private set; }
    public double GPSLongitude { get; private set; }
    public bool IsActive { get; private set; } = true;

    private readonly List<Department> _departments = new();
    public IReadOnlyCollection<Department> Departments => _departments.AsReadOnly();

    private readonly List<ScheduleHealthcareFacility> _schedules = new();
    public IReadOnlyCollection<ScheduleHealthcareFacility> Schedules => _schedules.AsReadOnly();

    private readonly List<ScheduleExceptionHealthcareFacility> _scheduleExceptionDays = new();
    public IReadOnlyCollection<ScheduleExceptionHealthcareFacility> ScheduleExceptions => _scheduleExceptionDays.AsReadOnly();

#pragma warning disable CS8618
    private HealthCareFacility() { }
#pragma warning restore CS8618

    private HealthCareFacility(Guid id, string userId, string name, HealthCareType type, Address address,
        double latitude, double longitude):base(id)
    {
        UserId = userId;
        Name = name;
        Type = type;
        Address = address;
        GPSLatitude = latitude;
        GPSLongitude = longitude;
        IsActive = true;
    }

    // ✅ Create with inline validation
    public static Result<HealthCareFacility> Create(
        Guid id,
        string userId,
        string name,
        HealthCareType type,
        Address address,
        double latitude,
        double longitude)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {

            return HealthCareFacilityErrors.UserIdRequired;
        }

        if (string.IsNullOrWhiteSpace(name))
        {

            return HealthCareFacilityErrors.NameRequired;
        }

        if (address is null)
        {

            return HealthCareFacilityErrors.AddressRequired;
        }

        return new HealthCareFacility(id, userId, name.Trim(), type, address, latitude, longitude);
    }

    public Result<Updated> Update(string name, HealthCareType type, Address address,
        double latitude, double longitude)
    {
        if (string.IsNullOrWhiteSpace(name))
        {

            return HealthCareFacilityErrors.NameRequired;
        }

        if (address is null)
        {

            return HealthCareFacilityErrors.AddressRequired;
        }

        Name = name.Trim();
        Type = type;
        Address = address;
        GPSLatitude = latitude;
        GPSLongitude = longitude;

        return Result.Updated;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public Result<Department> AddDepartment(string name, string description)
    {
        Result<Department> departmentResult = Department.Create(Id, name, description);
        if (departmentResult.IsError)
        {

            return departmentResult.Errors;
        }

        Department department = departmentResult.Value;
        _departments.Add(department);
        return department;
    }
}
