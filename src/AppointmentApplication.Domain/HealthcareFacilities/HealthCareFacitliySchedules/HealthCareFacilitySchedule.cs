using System;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.HealthcareFacilities.ScheduleHealthcareFacilities;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.HealthcareFacilities.Schedules;

public sealed class HealthCareFacilitySchedule : AuditableEntity
{
    public Guid FacilityId { get; private set; }
    public DaysOfWeek DayOfWeek { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public Status Status { get; private set; }
    public bool IsAvailable { get; private set; } = true;
    public string Note { get; private set; } = string.Empty;

    public HealthCareFacility Facility { get; private set; } = null!;

#pragma warning disable CS8618
    private HealthCareFacilitySchedule() { }
#pragma warning restore CS8618

    private HealthCareFacilitySchedule(Guid id, Guid facilityId, DaysOfWeek dayOfWeek, TimeSpan startTime,
        TimeSpan endTime, Status status, string note)
        : base(id)
    {
        FacilityId = facilityId;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
        IsAvailable = true;
        Note = note;
    }

    // ✅ Create
    public static Result<HealthCareFacilitySchedule> Create(
        Guid facilityId,
        DaysOfWeek dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime,
        Status status,
        bool isAvailable,
        string? note)
    {
        if (facilityId == Guid.Empty)
        {
            return HealthCareFacilityScheduleErrors.FacilityIdRequired;
        }

        if (!Enum.IsDefined(dayOfWeek))
        {
            return HealthCareFacilityScheduleErrors.InvalidDayOfWeek;
        }

        if (endTime <= startTime)
        {
            return HealthCareFacilityScheduleErrors.InvalidTimeRange;
        }

        if (!Enum.IsDefined(status))
        {
            return HealthCareFacilityScheduleErrors.StatusRequired;
        }

        if (!string.IsNullOrWhiteSpace(note) && note.Length > 500)
        {
            return HealthCareFacilityScheduleErrors.NoteTooLong;
        }

        return new HealthCareFacilitySchedule(
            Guid.NewGuid(),
            facilityId,
            dayOfWeek,
            startTime,
            endTime,
            status,
            note?.Trim() ?? string.Empty);
    }

    // ✅ Update
    public Result<Updated> Update(DaysOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime,
        Status status, bool isAvailable, string? note)
    {
        if (!Enum.IsDefined(dayOfWeek))
        {
            return HealthCareFacilityScheduleErrors.InvalidDayOfWeek;
        }

        if (endTime <= startTime)
        {
            return HealthCareFacilityScheduleErrors.InvalidTimeRange;
        }

        if (!Enum.IsDefined(status))
        {
            return HealthCareFacilityScheduleErrors.StatusRequired;
        }

        if (!string.IsNullOrWhiteSpace(note) && note.Length > 500)
        {
            return HealthCareFacilityScheduleErrors.NoteTooLong;
        }

        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
        Note = note?.Trim() ?? string.Empty;

        return Result.Updated;
    }

}
