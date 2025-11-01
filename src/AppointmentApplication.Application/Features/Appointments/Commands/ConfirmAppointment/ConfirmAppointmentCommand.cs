using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Appointments.Commands.ConfirmAppointment
{
    public record ConfirmAppointmentCommand(Guid UserId, Guid AppointmentId) : IRequest<Result<Updated>>;
}