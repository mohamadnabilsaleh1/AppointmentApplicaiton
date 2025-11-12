using System;

using AppointmentApplication.Application.Features.Appointments.Dtos;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Appointments.Commands.CreateAppointment;

// في CreateAppointmentCommand
public sealed record CreateAppointmentByDoctorCommand(
    Guid UserId,
    Guid PatientId,
    DateOnly ScheduledDate,
    TimeSpan ScheduledTime,
    int DurationMinutes,
    decimal? TotalAmount = null,
    string? Notes = null // ✅ إضافة Notes
) : IRequest<Result<AppointmentDto>>;