using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.DepatmentDoctors.Commands.AddDoctorToDepartment
{
    public sealed record AddDoctorToDepartmentCommand(Guid UserId,Guid DoctorId, Guid DepartmentId) : IRequest<Result<Success>>;

}