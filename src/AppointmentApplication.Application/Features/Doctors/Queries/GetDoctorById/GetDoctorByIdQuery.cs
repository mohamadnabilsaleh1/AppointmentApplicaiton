using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.Queries.GetDoctorsById
{
    public sealed record class GetDoctorByIdQuery(Guid DoctorId) : IRequest<Result<DoctorDto>>;
}