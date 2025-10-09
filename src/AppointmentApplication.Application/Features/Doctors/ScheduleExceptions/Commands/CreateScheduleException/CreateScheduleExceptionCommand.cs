// 📁 Application/HealthcareFacilities/ScheduleExceptions/Commands/CreateScheduleExceptionCommand.cs
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.ScheduleExceptions.Mappers;
public sealed record CreateScheduleExceptionCommand(
    Guid UserId,
    DateOnly Date,
    TimeSpan StartTime,
    TimeSpan EndTime,
    Status Status,
    string? Reason = null) : IRequest<Result<ScheduleExceptionDto>>;