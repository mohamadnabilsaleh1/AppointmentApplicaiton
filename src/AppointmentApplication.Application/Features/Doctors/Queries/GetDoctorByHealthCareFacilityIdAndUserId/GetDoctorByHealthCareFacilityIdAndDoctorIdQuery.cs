using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.Queries.GetDoctorByHealthCareFacilityIdAndUserId
{
    public sealed record GetDoctorByHealthCareFacilityIdAndDoctorIdQuery(Guid HealthCareFacilityId, Guid DoctorId) : IRequest<Result<DoctorDto>>;
}