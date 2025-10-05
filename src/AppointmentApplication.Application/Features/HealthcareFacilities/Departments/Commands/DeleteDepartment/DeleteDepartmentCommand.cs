using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Commands.DeleteDepartment
{
    public sealed record DeleteDepartmentCommand(Guid UserId, Guid DepartmentId):IRequest<Result<Deleted>>;
}