using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.HealthcareFacilities.Enums;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;

public record
ScheduleExceptionDto(
    Guid Id,
    Guid HealthcareFacilityId,
    DateOnly Date,
    DaysOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string Status,
    string Reason
);