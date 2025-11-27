using System;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Appointments.Commands.RescheduleAppointment
{
    public record RescheduleAppointmentByPatientIdCommand(
        Guid UserId,
        Guid AppointmentId,
        DateOnly NewDate,
        TimeSpan NewTime) : IRequest<Result<Updated>>;
}
