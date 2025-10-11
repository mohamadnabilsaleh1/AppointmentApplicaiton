using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Patients.Allergies.Enums;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Patients.Allergies.Commands.DeleteAllergy
{
    public sealed record DeleteAllergyCommand(Guid UserId, Guid
    AllergyId) : IRequest<Result<Deleted>>;
}