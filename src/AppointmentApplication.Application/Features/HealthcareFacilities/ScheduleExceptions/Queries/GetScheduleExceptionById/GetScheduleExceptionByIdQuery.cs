// 📁 Application/HealthcareFacilities/ScheduleExceptions/Queries/GetScheduleExceptionByIdQuery.cs
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Queries;

public sealed record GetScheduleExceptionByIdQuery(
    Guid HealthCareFacilityId,
    Guid ScheduleExceptionId) : IRequest<Result<ScheduleExceptionDto>>;