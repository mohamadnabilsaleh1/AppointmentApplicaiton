using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.Commands.UpdateDoctor
{
    public sealed record UpdateDoctorCommand(Guid UserId, string FirstName,
        string LastName, Gender Gender, DateOnly DateOfBirth) : IRequest<Result<Updated>>;

}