using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Domain.HealthcareFacilities.Schedules;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Mappers
{
    public static class SchedualMapper
    {
        public static ScheduleDto ToDto(this HealthCareFacilitySchedule entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new ScheduleDto(
                entity.Id,
                entity.DayOfWeek,
                entity.StartTime,
                entity.EndTime,
                entity.Note);
        }

        public static List<ScheduleDto> ToDtos(this IEnumerable<HealthCareFacilitySchedule> entities)
        {
            return entities.Select(e => e.ToDto()).ToList();
        }
    }
}