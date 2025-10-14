using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.DepatmentDoctors.Commands.DeleteDoctorFromDepartment
{
    public sealed record DeleteDoctorFromDepartmentCommand(Guid UserId, Guid DoctorId, Guid DepartmentId) : IRequest<Result<Deleted>>;

}