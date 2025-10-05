// 📁 Application/HealthcareFacilities/ScheduleExceptions/Commands/UpdateScheduleExceptionCommand.cs
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Commands;

public sealed record UpdateScheduleExceptionCommand(
    Guid UserId,
    Guid ExceptionId,
    DateOnly Date,
    TimeSpan StartTime,
    TimeSpan EndTime,
    Status Status,
    string? Reason = null) : IRequest<Result<Updated>>;