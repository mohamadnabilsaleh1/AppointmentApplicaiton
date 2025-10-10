using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Dtos;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Patients.Queries.GetPatientByUserId
{
    public sealed record GetPatientByUserIdQuery(Guid UserId) : IRequest<Result<PatientDto>>;
}