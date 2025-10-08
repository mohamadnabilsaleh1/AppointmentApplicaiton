// 📁 Application/HealthcareFacilities/Schedules/Commands/UpdateScheduleCommand.cs
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.Schedules.Commands;

public sealed record UpdateScheduleCommand(
    Guid UserId,
    Guid ScheduleId,
    DaysOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    Status Status,
    bool IsAvailable,
    string? Note = null) : IRequest<Result<Updated>>;