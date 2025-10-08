using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Doctors.DoctorScheduleExceptions
{
    public class DoctorScheduleExceptionErrors
    {
        public static readonly Error FacilityIdRequired =
    Error.Validation("DoctorScheduleException.DoctorIdRequired", "Doctor ID is required");

        public static readonly Error InvalidDate =
            Error.Validation("DoctorScheduleException.InvalidDate", "Invalid date");

        public static readonly Error InvalidDayOfWeek =
            Error.Validation("DoctorScheduleException.InvalidDayOfWeek", "Invalid day of week");

        public static readonly Error InvalidTimeRange =
            Error.Validation("DoctorScheduleException.InvalidTimeRange", "End time must be after start time");

        public static readonly Error StatusRequired =
            Error.Validation("DoctorScheduleException.StatusRequired", "Status is required");

        public static readonly Error ReasonTooLong =
            Error.Validation("DoctorScheduleException.ReasonTooLong", "Reason cannot exceed 500 characters");

        public static readonly Error ExceptionAlreadyExistsForDate =
            Error.Validation("DoctorScheduleException.ExceptionAlreadyExistsForDate", "An exception already exists for this date");

        public static readonly Error ScheduleExceptionNotFound =
            Error.NotFound("DoctorScheduleException.ScheduleExceptionNotFound", "Schedule exception not found");

        public static readonly Error ScheduleExceptionNotFoundForDate =
            Error.NotFound("DoctorScheduleException.ScheduleExceptionNotFoundForDate", "No schedule exception found for the specified date");
    }
}