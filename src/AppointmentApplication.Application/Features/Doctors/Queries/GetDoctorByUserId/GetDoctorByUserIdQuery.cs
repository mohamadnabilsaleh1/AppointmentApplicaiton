using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.Queries.GetDoctorByUserId
{
    public sealed record GetDoctorByUserIdQuery(Guid UserId) : IRequest<Result<DoctorDto>>;
}