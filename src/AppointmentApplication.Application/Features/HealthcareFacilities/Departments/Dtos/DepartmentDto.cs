using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Dtos;

public record DepartmentDto(
    Guid Id,
    string Name,
    string Description
);
