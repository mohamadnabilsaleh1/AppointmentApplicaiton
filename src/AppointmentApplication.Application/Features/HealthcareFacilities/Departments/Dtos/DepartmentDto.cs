using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Dtos;

public record DepartmentDto(
    Guid Id,
    Guid HealthcareFacilityId,
    string Name,
    string Description,
    bool IsActive,
    DateTime CreatedAt
);
