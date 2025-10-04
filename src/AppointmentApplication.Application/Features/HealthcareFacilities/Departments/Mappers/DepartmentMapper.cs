using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Dtos;
using AppointmentApplication.Domain.HealthcareFacilities.Departments;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Mappers;

public static class DepartmentMapper
{
    public static DepartmentDto ToDto(this Department entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new DepartmentDto(
            entity.Id,
            entity.FacilityId,
            entity.Name,
            entity.Description,
            entity.IsActive,
            entity.CreatedAtUtc);
    }

    public static List<DepartmentDto> ToDtos(this IEnumerable<Department> entities)
    {
        return entities.Select(e => e.ToDto()).ToList();
    }
}
