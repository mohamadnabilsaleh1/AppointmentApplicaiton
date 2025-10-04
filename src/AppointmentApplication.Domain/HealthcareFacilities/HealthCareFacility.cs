using System;
using System.Collections.Generic;
using System.Linq;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities.Departments;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.HealthcareFacilities.ScheduleExceptionHealthcareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities.ScheduleExceptions;
using AppointmentApplication.Domain.HealthcareFacilities.ScheduleHealthcareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities.Schedules;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;

namespace AppointmentApplication.Domain.HealthcareFacilities;

public sealed class HealthCareFacility : AuditableEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public string Name { get; private set; }
    public HealthCareType Type { get; private set; }
    public Address Address { get; private set; }
    public double GPSLatitude { get; private set; }
    public double GPSLongitude { get; private set; }
    public bool IsActive { get; private set; } = true;

    private readonly List<Department> _departments = new();
    public IReadOnlyCollection<Department> Departments => _departments.AsReadOnly();

    private readonly List<HealthCareFacilitySchedule> _schedules = new();
    public IReadOnlyCollection<HealthCareFacilitySchedule> Schedules => _schedules.AsReadOnly();

    private readonly List<HealthCareFacilityScheduleException> _scheduleExceptions = new();
    public IReadOnlyCollection<HealthCareFacilityScheduleException> ScheduleExceptions => _scheduleExceptions.AsReadOnly();

    private readonly List<Doctor> _doctors = new();
    public IReadOnlyCollection<Doctor> Doctors => _doctors.AsReadOnly();

#pragma warning disable CS8618
    private HealthCareFacility() { }
#pragma warning restore CS8618

    private HealthCareFacility(Guid id, Guid userId, string name, HealthCareType type, Address address,
        double latitude, double longitude)
        : base(id)
    {
        UserId = userId;
        Name = name;
        Type = type;
        Address = address;
        GPSLatitude = latitude;
        GPSLongitude = longitude;
        IsActive = true;
    }

    // ✅ Factory method with validation
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

        if (!IsValidCoordinates(latitude, longitude))
        {
            return HealthCareFacilityErrors.InvalidCoordinates;
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

        if (!IsValidCoordinates(latitude, longitude))
        {
            return HealthCareFacilityErrors.InvalidCoordinates;
        }

        Name = name.Trim();
        Address = address;
        GPSLatitude = latitude;
        GPSLongitude = longitude;
        return Result.Updated;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    // ✅ Add Schedule
    public Result<HealthCareFacilitySchedule> AddSchedule(
        DaysOfWeek dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime,
        Status status,
        string? note = null)
    {
        if (_schedules.Any(s => s.DayOfWeek == dayOfWeek))
        {
            return HealthCareFacilityScheduleErrors.ScheduleAlreadyExistsForDay;
        }

        if (endTime <= startTime)
        {
            return HealthCareFacilityScheduleErrors.InvalidTimeRange;
        }

        var duration = endTime - startTime;
        if (duration > TimeSpan.FromHours(24))
        {
            return HealthCareFacilityScheduleErrors.InvalidDuration;
        }

        var scheduleResult = HealthCareFacilitySchedule.Create(
            Id, dayOfWeek, startTime, endTime, status, true, note);

        if (scheduleResult.IsError)
        {
            return scheduleResult.Errors;
        }

        var schedule = scheduleResult.Value;

        if (HasScheduleOverlap(schedule))
        {
            return HealthCareFacilityScheduleErrors.ScheduleOverlap;
        }

        _schedules.Add(schedule);

        return schedule;
    }

    // ✅ Update Schedule
    public Result<Updated> UpdateSchedule(
        Guid scheduleId,
        DaysOfWeek dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime,
        Status status,
        bool isAvailable,
        string? note = null)
    {
        var schedule = _schedules.FirstOrDefault(s => s.Id == scheduleId);
        if (schedule == null)
        {
            return HealthCareFacilityScheduleErrors.ScheduleNotFound;
        }

        // Check if another schedule already exists for the target day (excluding current schedule)
        if (dayOfWeek != schedule.DayOfWeek && _schedules.Any(s => s.Id != scheduleId && s.DayOfWeek == dayOfWeek))
        {
            return HealthCareFacilityScheduleErrors.ScheduleAlreadyExistsForDay;
        }

        if (endTime <= startTime)
        {
            return HealthCareFacilityScheduleErrors.InvalidTimeRange;
        }

        var duration = endTime - startTime;
        if (duration > TimeSpan.FromHours(24))
        {
            return HealthCareFacilityScheduleErrors.InvalidDuration;
        }

        // Create a temporary schedule to check for overlaps
        var tempScheduleResult = HealthCareFacilitySchedule.Create(
            Guid.NewGuid(), dayOfWeek, startTime, endTime, status, isAvailable, note);

        if (tempScheduleResult.IsError)
        {
            return tempScheduleResult.Errors;
        }

        var tempSchedule = tempScheduleResult.Value;

        // Check for overlaps with other schedules (excluding current schedule)
        if (_schedules.Any(existingSchedule =>
            existingSchedule.Id != scheduleId &&
            existingSchedule.DayOfWeek == dayOfWeek &&
            existingSchedule.IsAvailable &&
            existingSchedule.StartTime < tempSchedule.EndTime &&
            tempSchedule.StartTime < existingSchedule.EndTime))
        {
            return HealthCareFacilityScheduleErrors.ScheduleOverlap;
        }

        // Update the schedule
        var updateResult = schedule.Update(dayOfWeek, startTime, endTime, status, isAvailable, note);
        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }

        return Result.Updated;
    }

    // ✅ Remove Schedule
    public Result<Deleted> RemoveSchedule(Guid scheduleId)
    {
        var schedule = _schedules.FirstOrDefault(s => s.Id == scheduleId);
        if (schedule == null)
        {
            return HealthCareFacilityScheduleErrors.ScheduleNotFound;
        }

        _schedules.Remove(schedule);
        return Result.Deleted;
    }

    // ✅ Get Schedule by Day
    public Result<HealthCareFacilitySchedule> GetScheduleByDay(DaysOfWeek dayOfWeek)
    {
        var schedule = _schedules.FirstOrDefault(s => s.DayOfWeek == dayOfWeek);
        if (schedule == null)
        {
            return HealthCareFacilityScheduleErrors.ScheduleNotFoundForDay;
        }

        return schedule;
    }

    // ✅ Get Schedule by ID
    public Result<HealthCareFacilitySchedule> GetScheduleById(Guid scheduleId)
    {
        var schedule = _schedules.FirstOrDefault(s => s.Id == scheduleId);
        if (schedule == null)
        {
            return HealthCareFacilityScheduleErrors.ScheduleNotFound;
        }

        return schedule;
    }

    // ✅ Check if facility is open on a specific day and time
    public Result<bool> IsOpenOn(DaysOfWeek dayOfWeek, TimeSpan time)
    {
        var schedule = _schedules.FirstOrDefault(s => s.DayOfWeek == dayOfWeek);
        if (schedule == null || !schedule.IsAvailable)
        {
            return false;
        }

        return time >= schedule.StartTime && time <= schedule.EndTime;
    }

    public Result<HealthCareFacilityScheduleException> AddScheduleException(
        DateOnly date,
        TimeSpan startTime,
        TimeSpan endTime,
        Status status,
        string? reason = null)
    {
        // Check if exception already exists for this date
        if (_scheduleExceptions.Any(se => se.Date == date))
        {
            return HealthCareFacilityScheduleExceptionErrors.ExceptionAlreadyExistsForDate;
        }

        if (endTime <= startTime)
        {
            return HealthCareFacilityScheduleExceptionErrors.InvalidTimeRange;
        }

        var dayOfWeek = GetDayOfWeekFromDate(date);

        var exceptionResult = HealthCareFacilityScheduleException.Create(
            Id, date, dayOfWeek, startTime, endTime, status, reason);

        if (exceptionResult.IsError)
        {
            return exceptionResult.Errors;
        }

        var scheduleException = exceptionResult.Value;
        _scheduleExceptions.Add(scheduleException);

        return scheduleException;
    }

    public Result<Updated> UpdateScheduleException(
        Guid exceptionId,
        DateOnly date,
        TimeSpan startTime,
        TimeSpan endTime,
        Status status,
        string? reason = null)
    {
        var scheduleException = _scheduleExceptions.FirstOrDefault(se => se.Id == exceptionId);
        if (scheduleException == null)
        {
            return HealthCareFacilityScheduleExceptionErrors.ScheduleExceptionNotFound;
        }

        // Check if another exception already exists for the target date (excluding current exception)
        if (date != scheduleException.Date && _scheduleExceptions.Any(se => se.Id != exceptionId && se.Date == date))
        {
            return HealthCareFacilityScheduleExceptionErrors.ExceptionAlreadyExistsForDate;
        }

        if (endTime <= startTime)
        {
            return HealthCareFacilityScheduleExceptionErrors.InvalidTimeRange;
        }

        var dayOfWeek = GetDayOfWeekFromDate(date);

        var updateResult = scheduleException.Update(date, dayOfWeek, startTime, endTime, status, reason);
        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }

        return Result.Updated;
    }

    public Result<Deleted> RemoveScheduleException(Guid exceptionId)
    {
        var scheduleException = _scheduleExceptions.FirstOrDefault(se => se.Id == exceptionId);
        if (scheduleException == null)
        {
            return HealthCareFacilityScheduleExceptionErrors.ScheduleExceptionNotFound;
        }

        _scheduleExceptions.Remove(scheduleException);
        return Result.Deleted;
    }

    public Result<HealthCareFacilityScheduleException> GetScheduleExceptionByDate(DateOnly date)
    {
        var scheduleException = _scheduleExceptions.FirstOrDefault(se => se.Date == date);
        if (scheduleException == null)
        {
            return HealthCareFacilityScheduleExceptionErrors.ScheduleExceptionNotFoundForDate;
        }

        return scheduleException;
    }

    public Result<HealthCareFacilityScheduleException> GetScheduleExceptionById(Guid exceptionId)
    {
        var scheduleException = _scheduleExceptions.FirstOrDefault(se => se.Id == exceptionId);
        if (scheduleException == null)
        {
            return HealthCareFacilityScheduleExceptionErrors.ScheduleExceptionNotFound;
        }

        return scheduleException;
    }

    public bool HasScheduleExceptionForDate(DateOnly date)
    {
        return _scheduleExceptions.Any(se => se.Date == date);
    }

    private static DaysOfWeek GetDayOfWeekFromDate(DateOnly date)
    {
        return date.DayOfWeek switch
        {
            DayOfWeek.Sunday => DaysOfWeek.Sunday,
            DayOfWeek.Monday => DaysOfWeek.Monday,
            DayOfWeek.Tuesday => DaysOfWeek.Tuesday,
            DayOfWeek.Wednesday => DaysOfWeek.Wednesday,
            DayOfWeek.Thursday => DaysOfWeek.Thursday,
            DayOfWeek.Friday => DaysOfWeek.Friday,
            DayOfWeek.Saturday => DaysOfWeek.Saturday,
            _ => DaysOfWeek.Sunday
        };
    }

    private bool HasScheduleOverlap(HealthCareFacilitySchedule newSchedule)
    {
        return _schedules.Any(existingSchedule =>
            existingSchedule.Id != newSchedule.Id &&
            existingSchedule.DayOfWeek == newSchedule.DayOfWeek &&
            existingSchedule.IsAvailable &&
            existingSchedule.StartTime < newSchedule.EndTime &&
            newSchedule.StartTime < existingSchedule.EndTime);
    }

    // ✅ Encapsulated validation
    private static bool IsValidCoordinates(double latitude, double longitude)
    {
        return latitude >= -90 && latitude <= 90 &&
               longitude >= -180 && longitude <= 180;
    }
}