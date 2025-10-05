using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Dtos;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Queries.GetDepartmentByUserId
{
    public sealed record GetDepartmentByUserIdQuery(Guid UserId, Guid DepartmentId) : IRequest<Result<DepartmentDto>>;
}