// 📁 Application/HealthcareFacilities/ScheduleExceptions/Commands/DeleteScheduleExceptionCommand.cs
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.ScheduleExceptions.Mappers;
public sealed record DeleteScheduleExceptionCommand(
    Guid UserId,
    Guid ExceptionId) : IRequest<Result<Deleted>>;