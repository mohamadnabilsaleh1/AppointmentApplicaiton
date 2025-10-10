using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Patients.Allergies.Enums;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Patients.Commands.AddAllergy
{
    public sealed record AddAllergyCommand(AllergyType
    AllergyType, Guid UserId) : IRequest<Result<Created>>;
}