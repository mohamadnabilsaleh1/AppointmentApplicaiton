using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Commands.UpdateDepartment
{
    public sealed record UpdateDepartmentCommand(Guid UserId,Guid DepartmentId, string Name, string Description) : IRequest<Result<Updated>>;
}