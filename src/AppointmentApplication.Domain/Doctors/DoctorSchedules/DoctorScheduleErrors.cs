using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Doctors.DoctorSchedules
{
    public class DoctorScheduleErrors
    {
        public static readonly Error DoctorIdRequired =
    Error.Validation("Doctor.DoctorIdRequired", "Doctor ID is required");

        public static readonly Error InvalidDayOfWeek =
            Error.Validation("DoctorSchedule.InvalidDayOfWeek", "Invalid day of week");

        public static readonly Error InvalidTimeRange =
            Error.Validation("DoctorSchedule.InvalidTimeRange", "End time must be after start time");

        public static readonly Error StatusRequired =
            Error.Validation("DoctorSchedule.StatusRequired", "Status is required");

        public static readonly Error NoteTooLong =
            Error.Validation("DoctorSchedule.NoteTooLong", "Note cannot exceed 500 characters");

        public static readonly Error ScheduleAlreadyExistsForDay =
            Error.Validation("DoctorSchedule.ScheduleAlreadyExistsForDay", "A schedule already exists for this day");

        public static readonly Error ScheduleOverlap =
            Error.Validation("DoctorSchedule.ScheduleOverlap", "Schedule overlaps with existing schedule");

        public static readonly Error ScheduleNotFound =
            Error.NotFound("DoctorSchedule.ScheduleNotFound", "Schedule not found");

        public static readonly Error ScheduleNotFoundForDay =
            Error.NotFound("DoctorSchedule.ScheduleNotFoundForDay", "No schedule found for the specified day");

        public static readonly Error InvalidDuration =
            Error.Validation("DoctorSchedule.InvalidDuration", "Schedule duration cannot exceed 24 hours");

        public static readonly Error WeekendNotAllowed =
            Error.Validation("Schedule.WeekendNotAllowed", "This facility cannot operate on weekends");
    }
}