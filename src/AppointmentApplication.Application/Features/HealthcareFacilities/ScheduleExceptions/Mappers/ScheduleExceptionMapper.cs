using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Domain.HealthcareFacilities.ScheduleExceptions;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Mappers;

public static class ScheduleExceptionMapper
{
    public static ScheduleExceptionDto ToDto(this HealthCareFacilityScheduleException entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new ScheduleExceptionDto(
            entity.Id,
            entity.FacilityId,   // now consistent
            entity.Date,
            entity.DayOfWeek,
            entity.StartTime,
            entity.EndTime,
            entity.Status.ToString(),
            entity.Reason);
    }

    public static List<ScheduleExceptionDto> ToDtos(this IEnumerable<HealthCareFacilityScheduleException> entities)
    {
        return entities.Select(e => e.ToDto()).ToList();
    }
}
