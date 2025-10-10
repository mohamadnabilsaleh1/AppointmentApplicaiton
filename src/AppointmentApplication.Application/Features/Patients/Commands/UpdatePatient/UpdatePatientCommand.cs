using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Enums;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Patients.Commands.UpdatePatient
{

    public sealed record UpdatePatientCommand(string NationalId, Guid UserId, Gender Gender, DateOnly DateOfBirth) : IRequest<Result<Updated>>;
}