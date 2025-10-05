// 📁 Application/HealthcareFacilities/Schedules/Queries/GetAllSchedulesQuery.cs
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.HealthcareFacilities.Schedules.Queries;

public sealed record GetSchedulesQuery(
    Guid UserId) : IRequest<Result<List<ScheduleDto>>>;