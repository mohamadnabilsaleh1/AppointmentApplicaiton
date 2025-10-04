using System;
using System.Collections.Generic;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.HealthcareFacilities.Departments;
using AppointmentApplication.Domain.HealthcareFacilities.Schedules;
using AppointmentApplication.Domain.HealthcareFacilities.ScheduleExceptions;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.Users;
using AppointmentApplication.Domain.HealthcareFacilities.ScheduleHealthcareFacilities;
using AppointmentApplication.Domain.Doctors;

namespace AppointmentApplication.Domain.HealthcareFacilities;

public sealed class HealthCareFacility : AuditableEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; }
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
    private readonly List<Doctor> _doctors = new();
    public IReadOnlyCollection<Doctor> Doctors => _doctors.AsReadOnly();


#pragma warning disable CS8618
    private HealthCareFacility() { }
#pragma warning restore CS8618

    private HealthCareFacility(Guid id, Guid userId, string name, HealthCareType type, Address address,
        double latitude, double longitude) : base(id)
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
        Guid userId,
        string name,
        HealthCareType type,
        Address address,
        double latitude,
        double longitude)
    {
        if (userId == Guid.Empty)
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

    public Result<Updated> Update(string name, Address address,
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
        Address = address;
        GPSLatitude = latitude;
        GPSLongitude = longitude;

        return Result.Updated;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public Result<ScheduleHealthcareFacility> AddSchedule(
    DaysOfWeek dayOfWeek,
    TimeSpan startTime,
    TimeSpan endTime,
    Status status,
    string? note = null)
    {
        // 1. التحقق من وجود جدول مكرر لنفس اليوم
        if (_schedules.Any(s => s.DayOfWeek == dayOfWeek))
        {
            return ScheduleHealthcareFacilityErrors.ScheduleAlreadyExistsForDay;
        }

        // 2. التحقق من أن الوقت النهائي بعد الوقت البدائي
        if (endTime <= startTime)
        {
            return ScheduleHealthcareFacilityErrors.InvalidTimeRange;
        }

        // 3. التحقق من أن المدة معقولة (أقل من 24 ساعة)
        var duration = endTime - startTime;
        if (duration > TimeSpan.FromHours(24))
        {
            return ScheduleHealthcareFacilityErrors.InvalidDuration;
        }

        // 4. إنشاء الجدول الجديد - إضافة true للمعامل isAvailable
        var scheduleResult = ScheduleHealthcareFacility.Create(
            Id, dayOfWeek, startTime, endTime, status, true, note); // ✅ أضفت true هنا

        if (scheduleResult.IsError)
            return scheduleResult.Errors;

        var schedule = scheduleResult.Value;

        // 5. التحقق من عدم تعارض الجدول مع جداول أخرى
        if (HasScheduleOverlap(schedule))
        {
            return ScheduleHealthcareFacilityErrors.ScheduleOverlap;
        }

        // 6. إضافة الجدول إلى القائمة
        _schedules.Add(schedule);

        return schedule;
    }
    private bool HasScheduleOverlap(ScheduleHealthcareFacility newSchedule)
    {
        return _schedules.Any(existingSchedule =>
            existingSchedule.Id != newSchedule.Id && // استبعاد الجدول نفسه إذا كان يتم تحديثه
            existingSchedule.DayOfWeek == newSchedule.DayOfWeek &&
            existingSchedule.IsAvailable &&
            existingSchedule.StartTime < newSchedule.EndTime &&
            newSchedule.StartTime < existingSchedule.EndTime);
    }


}
