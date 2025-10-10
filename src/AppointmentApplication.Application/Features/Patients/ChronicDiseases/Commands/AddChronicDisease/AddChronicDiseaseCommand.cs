using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Patients.Allergies.Enums;
using AppointmentApplication.Domain.Patients.ChronicDiseases;
using AppointmentApplication.Domain.Patients.ChronicDiseases.Enums;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Patients.Commands.AddAllergy
{
    public sealed record AddChronicDiseaseCommand(ChronicDiseaseType
    ChronicDiseaseType, Guid UserId) : IRequest<Result<ChronicDisease>>;
}