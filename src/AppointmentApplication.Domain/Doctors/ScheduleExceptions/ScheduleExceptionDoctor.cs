using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.Domain.Doctors.ScheduleExceptions;

public class ScheduleExceptionDoctor:AuditableEntity
{
    private ScheduleExceptionDoctor() { }

    public Guid DoctorId { get; private set; }
    public DateTime Date { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public Status Status { get; private set; }
    public string Reason { get; private set; }

    public Doctor Doctor { get; private set; }

    public static ScheduleExceptionDoctor Create(Guid doctorId, DateTime date, DayOfWeek dayOfWeek,
        TimeSpan startTime, TimeSpan endTime, Status status, string reason)
    {
        return new ScheduleExceptionDoctor
        {
            DoctorId = doctorId,
            Date = date,
            DayOfWeek = dayOfWeek,
            StartTime = startTime,
            EndTime = endTime,
            Status = status,
            Reason = reason,
        };
    }

}
