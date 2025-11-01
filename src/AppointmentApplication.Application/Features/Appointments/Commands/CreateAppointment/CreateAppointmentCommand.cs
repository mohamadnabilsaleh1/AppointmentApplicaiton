using System;

using AppointmentApplication.Application.Features.Appointments.Dtos;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Appointments.Commands.CreateAppointment;

// في CreateAppointmentCommand
public sealed record CreateAppointmentCommand(
    Guid UserId,
    Guid DoctorId,
    Guid FacilityId,
    DateOnly ScheduledDate,
    TimeSpan ScheduledTime,
    int DurationMinutes,
    decimal? TotalAmount = null,
    string? Notes = null // ✅ إضافة Notes
) : IRequest<Result<AppointmentDto>>;