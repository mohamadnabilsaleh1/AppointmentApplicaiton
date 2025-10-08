using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Doctors.DoctorSchedules;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Doctors.Schedules;

public class DoctorSchedule : AuditableEntity
{
    public Guid DoctorId { get; private set; }
    public DaysOfWeek DayOfWeek { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public Status Status { get; private set; }
    public string Note { get; private set; } = string.Empty;
    public bool IsAvailable { get; private set; } = true;
    public Doctor? Doctor { get; set; }

    private DoctorSchedule(Guid id, Guid doctorId,
        DaysOfWeek dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime,
        Status status,
        string note)
        : base(id)
    {
        DoctorId = doctorId;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
        Note = note;
    }

    public static Result<DoctorSchedule> Create(
        Guid doctorId,
        DaysOfWeek dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime,
        Status status,
        string? note)
    {
        if (doctorId == Guid.Empty)
        {
            return DoctorScheduleErrors.DoctorIdRequired;
        }

        if (!Enum.IsDefined(dayOfWeek))
        {
            return DoctorScheduleErrors.InvalidDayOfWeek;
        }

        if (endTime <= startTime)
        {
            return DoctorScheduleErrors.InvalidTimeRange;
        }

        if (!Enum.IsDefined(status))
        {
            return DoctorScheduleErrors.StatusRequired;
        }

        if (!string.IsNullOrWhiteSpace(note) && note.Length > 500)
        {
            return DoctorScheduleErrors.NoteTooLong;
        }

        return new DoctorSchedule(
            Guid.NewGuid(),
            doctorId,
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
            return DoctorScheduleErrors.InvalidDayOfWeek;
        }

        if (endTime <= startTime)
        {
            return DoctorScheduleErrors.InvalidTimeRange;
        }

        if (!Enum.IsDefined(status))
        {
            return DoctorScheduleErrors.StatusRequired;
        }

        if (!string.IsNullOrWhiteSpace(note) && note.Length > 500)
        {
            return DoctorScheduleErrors.NoteTooLong;
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
