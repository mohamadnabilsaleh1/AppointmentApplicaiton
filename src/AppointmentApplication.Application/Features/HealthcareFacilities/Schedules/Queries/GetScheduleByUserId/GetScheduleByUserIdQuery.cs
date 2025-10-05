// 📁 Application/HealthcareFacilities/Schedules/Queries/GetScheduleByIdQuery.cs
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.HealthcareFacilities.Schedules.Queries;

public sealed record GetScheduleByUserIdQuery(
    Guid UserId,
    Guid ScheduleId) : IRequest<Result<ScheduleDto>>;