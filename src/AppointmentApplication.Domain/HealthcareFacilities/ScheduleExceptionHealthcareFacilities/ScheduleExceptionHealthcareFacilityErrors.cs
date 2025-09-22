using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.HealthcareFacilities.ScheduleExceptionHealthcareFacilities;

    public static class ScheduleExceptionHealthcareFacilityErrors
    {
        public static readonly Error FacilityIdRequired = 
            Error.Validation("ScheduleException.FacilityId.Required", "Facility ID is required.");

        public static readonly Error InvalidDate = 
            Error.Validation("ScheduleException.Date.Invalid", "Date cannot be default.");

        public static readonly Error InvalidDayOfWeek = 
            Error.Validation("ScheduleException.DayOfWeek.Invalid", "Invalid day of week.");

        public static readonly Error InvalidTimeRange = 
            Error.Validation("ScheduleException.TimeRange.Invalid", "End time must be after start time.");

        public static readonly Error StatusRequired = 
            Error.Validation("ScheduleException.Status.Required", "Invalid or missing status.");

        public static readonly Error ReasonTooLong = 
            Error.Validation("ScheduleException.Reason.TooLong", "Reason cannot exceed 500 characters.");
    }