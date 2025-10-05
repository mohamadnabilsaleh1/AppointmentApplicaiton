// 📁 Application/HealthcareFacilities/ScheduleExceptions/Commands/DeleteScheduleExceptionCommand.cs
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Commands;

public sealed record DeleteScheduleExceptionCommand(
    Guid UserId,
    Guid ExceptionId) : IRequest<Result<Deleted>>;