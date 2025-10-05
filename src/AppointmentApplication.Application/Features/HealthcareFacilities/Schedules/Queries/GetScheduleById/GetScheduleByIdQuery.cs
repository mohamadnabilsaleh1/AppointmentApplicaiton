// 📁 Application/HealthcareFacilities/Schedules/Queries/GetScheduleByIdQuery.cs
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.HealthcareFacilities.Schedules.Queries;

public sealed record GetScheduleByIdQuery(
    Guid HealthCareFacilityId,
    Guid ScheduleId) : IRequest<Result<ScheduleDto>>;