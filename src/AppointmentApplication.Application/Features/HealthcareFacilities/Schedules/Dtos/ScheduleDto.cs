using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.HealthcareFacilities.Enums;

using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;

public record ScheduleDto(
    Guid Id,
    DaysOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string Note
);