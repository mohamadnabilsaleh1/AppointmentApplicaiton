using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.HealthcareFacilities.ScheduleHealthcareFacilities;

public static class HealthCareFacilityScheduleErrors
{
    public static readonly Error FacilityIdRequired =
        Error.Validation("Schedule.FacilityId.Required", "Facility ID is required.");

    public static readonly Error InvalidDayOfWeek =
        Error.Validation("Schedule.DayOfWeek.Invalid", "Invalid day of week.");

    public static readonly Error InvalidTimeRange =
        Error.Validation("Schedule.TimeRange.Invalid", "End time must be after start time.");

    public static readonly Error StatusRequired =
        Error.Validation("Schedule.Status.Required", "Status is required.");

    public static readonly Error NoteTooLong =
        Error.Validation("Schedule.Note.TooLong", "Note cannot exceed 500 characters.");

    public static readonly Error ScheduleAlreadyExistsForDay =
        Error.Validation("Schedule.AlreadyExists", "A schedule already exists for this day");

    public static readonly Error NotFound =
        Error.NotFound("Schedule.NotFound", "Schedule not found");

    public static readonly Error FacilityInactive =
        Error.Validation("Schedule.FacilityInactive", "Cannot add schedule to an inactive facility");

    public static readonly Error ScheduleOverlap =
        Error.Validation("Schedule.Overlap", "This schedule overlaps with an existing schedule");

    public static readonly Error InvalidDuration =
        Error.Validation("Schedule.InvalidDuration", "Schedule duration cannot exceed 24 hours");

    public static readonly Error WeekendNotAllowed =
        Error.Validation("Schedule.WeekendNotAllowed", "This facility cannot operate on weekends");
}