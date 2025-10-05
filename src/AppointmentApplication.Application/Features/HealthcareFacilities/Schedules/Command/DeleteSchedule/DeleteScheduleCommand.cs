// 📁 Application/HealthcareFacilities/Schedules/Commands/DeleteScheduleCommand.cs
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.HealthcareFacilities.Schedules.Commands;

public sealed record DeleteScheduleCommand(
    Guid UserId,
    Guid ScheduleId) : IRequest<Result<Deleted>>;