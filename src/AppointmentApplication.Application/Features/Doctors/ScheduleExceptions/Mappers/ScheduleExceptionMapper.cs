using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.Doctors.ScheduleExceptions;
using AppointmentApplication.Domain.HealthcareFacilities.ScheduleExceptions;

namespace AppointmentApplication.Application.Features.Doctors.ScheduleExceptions.Mappers;

public static class ScheduleExceptionMapper
{
    public static ScheduleExceptionDto ToDto(this DoctorScheduleException entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new ScheduleExceptionDto(
            entity.Id,
            entity.DoctorId,   // now consistent
            entity.Date,
            entity.DayOfWeek,
            entity.StartTime,
            entity.EndTime,
            entity.Status.ToString(),
            entity.Reason);
    }

    public static List<ScheduleExceptionDto> ToDtos(this IEnumerable<DoctorScheduleException> entities)
    {
        return entities.Select(e => e.ToDto()).ToList();
    }
}
