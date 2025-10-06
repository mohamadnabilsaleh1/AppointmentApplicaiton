using System;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.HealthcareFacilities.ScheduleExceptionHealthcareFacilities;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.HealthcareFacilities.ScheduleExceptions;

public sealed class HealthCareFacilityScheduleException : AuditableEntity
{
    public Guid FacilityId { get; private set; }
    public DateOnly Date { get; private set; }
    public DaysOfWeek DayOfWeek { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public Status Status { get; private set; }
    public string Reason { get; private set; } = string.Empty;

    public HealthCareFacility? HealthCareFacility { get;  set; }

#pragma warning disable CS8618
    private HealthCareFacilityScheduleException() { }
#pragma warning restore CS8618

    private HealthCareFacilityScheduleException(Guid facilityId, DateOnly date, DaysOfWeek dayOfWeek,
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

    public static Result<HealthCareFacilityScheduleException> Create(
        Guid facilityId, DateOnly date, DaysOfWeek dayOfWeek, TimeSpan startTime,
        TimeSpan endTime, Status status, string? reason)
    {
        if (facilityId == Guid.Empty)
        {
            return HealthCareFacilityScheduleExceptionErrors.FacilityIdRequired;
        }

        if (date == default)
        {
            return HealthCareFacilityScheduleExceptionErrors.InvalidDate;
        }

        if (!Enum.IsDefined(dayOfWeek))
        {
            return HealthCareFacilityScheduleExceptionErrors.InvalidDayOfWeek;
        }

        if (endTime <= startTime)
        {
            return HealthCareFacilityScheduleExceptionErrors.InvalidTimeRange;
        }

        if (!Enum.IsDefined(status))
        {
            return HealthCareFacilityScheduleExceptionErrors.StatusRequired;
        }

        if (!string.IsNullOrWhiteSpace(reason) && reason.Length > 500)
        {
            return HealthCareFacilityScheduleExceptionErrors.ReasonTooLong;
        }

        return new HealthCareFacilityScheduleException(
            facilityId, date, dayOfWeek, startTime, endTime, status, reason?.Trim() ?? string.Empty);
    }

    public Result<Updated> Update(DateOnly date, DaysOfWeek dayOfWeek, TimeSpan startTime,
        TimeSpan endTime, Status status, string? reason)
    {
        if (date == default)
        {
            return HealthCareFacilityScheduleExceptionErrors.InvalidDate;
        }

        if (!Enum.IsDefined(dayOfWeek))
        {
            return HealthCareFacilityScheduleExceptionErrors.InvalidDayOfWeek;
        }

        if (endTime <= startTime)
        {
            return HealthCareFacilityScheduleExceptionErrors.InvalidTimeRange;
        }

        if (!Enum.IsDefined(status))
        {
            return HealthCareFacilityScheduleExceptionErrors.StatusRequired;
        }

        if (!string.IsNullOrWhiteSpace(reason) && reason.Length > 500)
        {
            return HealthCareFacilityScheduleExceptionErrors.ReasonTooLong;
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
