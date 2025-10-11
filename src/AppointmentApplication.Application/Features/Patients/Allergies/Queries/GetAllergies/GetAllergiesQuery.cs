using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Patients.Allergies;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Patients.Allergies.Queries.GetAllergies
{
    public sealed record GetAllergiesQuery(Guid UserId) : IRequest<Result<List<Allergy>>>;
}