using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.Domain.Doctors.Schedules;

public class ScheduleDoctor : AuditableEntity
{
    public Guid DoctorId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public Status Status { get; private set; }
    public bool IsAvailable { get; private set; } = true;
    public string Note { get; private set; } = string.Empty;
    public Doctor Doctor { get; private set; }

    public static ScheduleDoctor Create(Guid doctorId, DayOfWeek dayOfWeek, TimeSpan startTime, 
        TimeSpan endTime, Status status, bool isAvailable, string note )
    {
        return new ScheduleDoctor
        {
            DoctorId = doctorId,
            DayOfWeek = dayOfWeek,
            StartTime = startTime,
            EndTime = endTime,
            Status = status,
            IsAvailable = isAvailable,
            Note = note,
        };
    }

}
