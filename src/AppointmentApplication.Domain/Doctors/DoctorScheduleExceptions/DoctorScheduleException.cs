using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Doctors.DoctorScheduleExceptions;
using AppointmentApplication.Domain.Doctors.DoctorSchedules;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Doctors.ScheduleExceptions;

public class DoctorScheduleException : AuditableEntity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private DoctorScheduleException() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public Guid DoctorId { get; private set; }
    public DateOnly Date { get; private set; }
    public DaysOfWeek DayOfWeek { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public Status Status { get; private set; }
    public string Reason { get; private set; }

    public Doctor? Doctor { get; set; }

    private DoctorScheduleException(Guid doctorId, DateOnly date, DaysOfWeek dayOfWeek,
        TimeSpan startTime, TimeSpan endTime, Status status, string reason)
    {
        Id = Guid.NewGuid();
        DoctorId = doctorId;
        Date = date;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
        Reason = reason;
    }

    public static Result<DoctorScheduleException> Create(Guid doctorId, DateOnly date, DaysOfWeek dayOfWeek,
        TimeSpan startTime, TimeSpan endTime, Status status, string reason)
    {
        return new DoctorScheduleException(
            doctorId, date, dayOfWeek, startTime, endTime, status, reason);
    }

    public Result<Updated> Update(DateOnly date, DaysOfWeek dayOfWeek, TimeSpan startTime,
    TimeSpan endTime, Status status, string? reason)
    {
        if (date == default)
        {
            return DoctorScheduleExceptionErrors.InvalidDate;
        }

        if (!Enum.IsDefined(dayOfWeek))
        {
            return DoctorScheduleExceptionErrors.InvalidDayOfWeek;
        }

        if (endTime <= startTime)
        {
            return DoctorScheduleExceptionErrors.InvalidTimeRange;
        }

        if (!Enum.IsDefined(status))
        {
            return DoctorScheduleExceptionErrors.StatusRequired;
        }

        if (!string.IsNullOrWhiteSpace(reason) && reason.Length > 500)
        {
            return DoctorScheduleExceptionErrors.ReasonTooLong;
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
