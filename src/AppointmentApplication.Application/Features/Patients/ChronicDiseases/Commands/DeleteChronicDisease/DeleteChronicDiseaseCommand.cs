using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Patients.ChronicDiseases.Enums;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Patients.ChronicDiseases.Commands.DeleteChronicDisease
{
    public sealed record DeleteChronicDiseaseCommand(Guid ChronicDiseaseId, Guid UserId) : IRequest<Result<Deleted>>;
}