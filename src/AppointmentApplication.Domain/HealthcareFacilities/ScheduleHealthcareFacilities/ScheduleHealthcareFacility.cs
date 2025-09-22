using System;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.HealthcareFacilities.ScheduleHealthcareFacilities;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.HealthcareFacilities.Schedules;

public sealed class ScheduleHealthcareFacility : AuditableEntity
{
    public Guid FacilityId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public Status Status { get; private set; }
    public bool IsAvailable { get; private set; }
    public string Note { get; private set; } = string.Empty;

    public HealthCareFacility Facility { get; private set; }

#pragma warning disable CS8618
    private ScheduleHealthcareFacility() { }
#pragma warning restore CS8618

    private ScheduleHealthcareFacility(Guid id, Guid facilityId, DayOfWeek dayOfWeek, TimeSpan startTime,
        TimeSpan endTime, Status status, bool isAvailable, string note)
        : base(id)
    {
        FacilityId = facilityId;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
        IsAvailable = isAvailable;
        Note = note;
    }

    // ✅ Create
    public static Result<ScheduleHealthcareFacility> Create(
        Guid facilityId,
        DayOfWeek dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime,
        Status status,
        bool isAvailable,
        string? note)
    {
        if (facilityId == Guid.Empty)
        {

            return ScheduleHealthcareFacilityErrors.FacilityIdRequired;
        }


        if (!Enum.IsDefined(dayOfWeek))
        {

            return ScheduleHealthcareFacilityErrors.InvalidDayOfWeek;
        }


        if (endTime <= startTime)
        {

            return ScheduleHealthcareFacilityErrors.InvalidTimeRange;
        }


        if (!Enum.IsDefined(status))
        {

            return ScheduleHealthcareFacilityErrors.StatusRequired;
        }


        if (!string.IsNullOrWhiteSpace(note) && note.Length > 500)
        {

            return ScheduleHealthcareFacilityErrors.NoteTooLong;
        }


        return new ScheduleHealthcareFacility(
            Guid.NewGuid(),
            facilityId,
            dayOfWeek,
            startTime,
            endTime,
            status,
            isAvailable,
            note?.Trim() ?? string.Empty);
    }

    // ✅ Update
    public Result<Updated> Update(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime,
        Status status, bool isAvailable, string? note)
    {
        if (!Enum.IsDefined(dayOfWeek))
        {

            return ScheduleHealthcareFacilityErrors.InvalidDayOfWeek;
        }


        if (endTime <= startTime)
        {

            return ScheduleHealthcareFacilityErrors.InvalidTimeRange;
        }


        if (!Enum.IsDefined(status))
        {

            return ScheduleHealthcareFacilityErrors.StatusRequired;
        }


        if (!string.IsNullOrWhiteSpace(note) && note.Length > 500)
        {

            return ScheduleHealthcareFacilityErrors.NoteTooLong;
        }


        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
        IsAvailable = isAvailable;
        Note = note?.Trim() ?? string.Empty;

        return Result.Updated;
    }
}
