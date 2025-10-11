using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Patients.ChronicDiseases;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Patients.ChronicDiseases.Queries.GetChronicDiseases
{
    public sealed record GetChronicDiseasesQuery(Guid UserId) : IRequest<Result<List<ChronicDisease>>>;
}