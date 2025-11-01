using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Appointments.Commands.CancelAppointment
{
    public record CancelAppointmentCommand(Guid UserId, Guid AppointmentId, string CancellationReason) : IRequest<Result<Updated>>;
}