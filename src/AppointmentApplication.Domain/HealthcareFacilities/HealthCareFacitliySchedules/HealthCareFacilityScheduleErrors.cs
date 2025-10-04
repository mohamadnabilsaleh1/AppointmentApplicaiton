using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.HealthcareFacilities.ScheduleHealthcareFacilities;

public static class HealthCareFacilityScheduleErrors
{
    public static readonly Error FacilityIdRequired =
        Error.Validation("HealthCareFacilitySchedule.FacilityIdRequired", "Facility ID is required");

    public static readonly Error InvalidDayOfWeek =
        Error.Validation("HealthCareFacilitySchedule.InvalidDayOfWeek", "Invalid day of week");

    public static readonly Error InvalidTimeRange =
        Error.Validation("HealthCareFacilitySchedule.InvalidTimeRange", "End time must be after start time");

    public static readonly Error StatusRequired =
        Error.Validation("HealthCareFacilitySchedule.StatusRequired", "Status is required");

    public static readonly Error NoteTooLong =
        Error.Validation("HealthCareFacilitySchedule.NoteTooLong", "Note cannot exceed 500 characters");

    public static readonly Error ScheduleAlreadyExistsForDay =
        Error.Validation("HealthCareFacilitySchedule.ScheduleAlreadyExistsForDay", "A schedule already exists for this day");

    public static readonly Error ScheduleOverlap =
        Error.Validation("HealthCareFacilitySchedule.ScheduleOverlap", "Schedule overlaps with existing schedule");

    public static readonly Error ScheduleNotFound =
        Error.NotFound("HealthCareFacilitySchedule.ScheduleNotFound", "Schedule not found");

    public static readonly Error ScheduleNotFoundForDay =
        Error.NotFound("HealthCareFacilitySchedule.ScheduleNotFoundForDay", "No schedule found for the specified day");

    public static readonly Error InvalidDuration =
        Error.Validation("HealthCareFacilitySchedule.InvalidDuration", "Schedule duration cannot exceed 24 hours");

    public static readonly Error WeekendNotAllowed =
        Error.Validation("Schedule.WeekendNotAllowed", "This facility cannot operate on weekends");
}