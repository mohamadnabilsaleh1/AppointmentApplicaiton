using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Appointments.Dtos;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Appointments.Queries.GetAppointmentDetailsForDoctorById
{
    public sealed record GetAppointmentDetailsForDoctorByIdQuery(
        Guid UserId,
        Guid AppointmentId,
        string? Fields = null
    ) : IRequest<Result<AppointmentDetailsDto>>;
}