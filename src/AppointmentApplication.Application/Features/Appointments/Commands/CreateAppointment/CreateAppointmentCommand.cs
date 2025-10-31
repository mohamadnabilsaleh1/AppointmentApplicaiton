using System;
using AppointmentApplication.Application.Features.Appointments.Dtos;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Appointments.Commands.CreateAppointment
{
    public sealed record CreateAppointmentCommand(
        Guid UserId,
        Guid DoctorId,
        Guid FacilityId,
        DateOnly ScheduledDate,
        TimeSpan ScheduledTime,
        int DurationMinutes,
        string Notes,
        decimal? TotalAmount = null
    ) : IRequest<Result<Guid>>;
}