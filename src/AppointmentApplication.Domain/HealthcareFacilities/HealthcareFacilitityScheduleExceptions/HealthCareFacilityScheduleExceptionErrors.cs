using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.HealthcareFacilities.ScheduleExceptionHealthcareFacilities;

public static class HealthCareFacilityScheduleExceptionErrors
{
    public static readonly Error FacilityIdRequired =
        Error.Validation("HealthCareFacilityScheduleException.FacilityIdRequired", "Facility ID is required");

    public static readonly Error InvalidDate =
        Error.Validation("HealthCareFacilityScheduleException.InvalidDate", "Invalid date");

    public static readonly Error InvalidDayOfWeek =
        Error.Validation("HealthCareFacilityScheduleException.InvalidDayOfWeek", "Invalid day of week");

    public static readonly Error InvalidTimeRange =
        Error.Validation("HealthCareFacilityScheduleException.InvalidTimeRange", "End time must be after start time");

    public static readonly Error StatusRequired =
        Error.Validation("HealthCareFacilityScheduleException.StatusRequired", "Status is required");

    public static readonly Error ReasonTooLong =
        Error.Validation("HealthCareFacilityScheduleException.ReasonTooLong", "Reason cannot exceed 500 characters");

    public static readonly Error ExceptionAlreadyExistsForDate =
        Error.Validation("HealthCareFacilityScheduleException.ExceptionAlreadyExistsForDate", "An exception already exists for this date");

    public static readonly Error ScheduleExceptionNotFound =
        Error.NotFound("HealthCareFacilityScheduleException.ScheduleExceptionNotFound", "Schedule exception not found");

    public static readonly Error ScheduleExceptionNotFoundForDate =
        Error.NotFound("HealthCareFacilityScheduleException.ScheduleExceptionNotFoundForDate", "No schedule exception found for the specified date");
}