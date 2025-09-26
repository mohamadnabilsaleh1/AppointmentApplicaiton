using System;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.HealthcareFacilities.ScheduleExceptionHealthcareFacilities;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.HealthcareFacilities.ScheduleExceptions;

public sealed class ScheduleExceptionHealthcareFacility : AuditableEntity
{
    public Guid FacilityId { get; private set; }
    public DateOnly Date { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public Status Status { get; private set; }
    public string Reason { get; private set; } = string.Empty;

    public HealthCareFacility Facility { get; private set; } = default!;

#pragma warning disable CS8618
    private ScheduleExceptionHealthcareFacility() { }
#pragma warning restore CS8618

    private ScheduleExceptionHealthcareFacility(Guid facilityId, DateOnly date, DayOfWeek dayOfWeek,
        TimeSpan startTime, TimeSpan endTime, Status status, string reason)
    {
        Id = Guid.NewGuid();
        FacilityId = facilityId;
        Date = date;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
        Reason = reason;
    }

    public static Result<ScheduleExceptionHealthcareFacility> Create(
        Guid facilityId, DateOnly date, DayOfWeek dayOfWeek, TimeSpan startTime,
        TimeSpan endTime, Status status, string? reason)
    {
        if (facilityId == Guid.Empty)
        {

            return ScheduleExceptionHealthcareFacilityErrors.FacilityIdRequired;
        }

        if (date == default)
        {

            return ScheduleExceptionHealthcareFacilityErrors.InvalidDate;
        }

        if (!Enum.IsDefined(dayOfWeek))
        {

            return ScheduleExceptionHealthcareFacilityErrors.InvalidDayOfWeek;
        }

        if (endTime <= startTime)
        {

            return ScheduleExceptionHealthcareFacilityErrors.InvalidTimeRange;
        }

        if (!Enum.IsDefined(status))
        {

            return ScheduleExceptionHealthcareFacilityErrors.StatusRequired;
        }

        if (!string.IsNullOrWhiteSpace(reason) && reason.Length > 500)
        {

            return ScheduleExceptionHealthcareFacilityErrors.ReasonTooLong;
        }

        return new ScheduleExceptionHealthcareFacility(
            facilityId, date, dayOfWeek, startTime, endTime, status, reason?.Trim() ?? string.Empty);
    }

    public Result<Updated> Update(DateOnly date, DayOfWeek dayOfWeek, TimeSpan startTime,
        TimeSpan endTime, Status status, string? reason)
    {
        if (date == default)
        {

            return ScheduleExceptionHealthcareFacilityErrors.InvalidDate;
        }

        if (!Enum.IsDefined(dayOfWeek))
        {

            return ScheduleExceptionHealthcareFacilityErrors.InvalidDayOfWeek;
        }

        if (endTime <= startTime)
        {

            return ScheduleExceptionHealthcareFacilityErrors.InvalidTimeRange;
        }

        if (!Enum.IsDefined(status))
        {

            return ScheduleExceptionHealthcareFacilityErrors.StatusRequired;
        }

        if (!string.IsNullOrWhiteSpace(reason) && reason.Length > 500)
        {

            return ScheduleExceptionHealthcareFacilityErrors.ReasonTooLong;
        }

        Date = date;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
        Reason = reason?.Trim() ?? string.Empty;

        return Result.Updated;
    }
}
