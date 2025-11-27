using System;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Appointments.Commands.CancelAppointment
{
    public record CancelAppointmentByPatientIdCommand(Guid UserId, Guid AppointmentId, string CancellationReason)
        : IRequest<Result<Updated>>;
}
