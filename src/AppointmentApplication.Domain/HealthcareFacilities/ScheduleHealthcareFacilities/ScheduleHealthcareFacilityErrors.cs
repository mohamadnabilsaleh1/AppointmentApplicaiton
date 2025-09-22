using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.HealthcareFacilities.ScheduleHealthcareFacilities;

    public static class ScheduleHealthcareFacilityErrors
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
    }