// 📁 Application/HealthcareFacilities/ScheduleExceptions/Queries/GetScheduleExceptionByIdQuery.cs
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Queries;

public sealed record GetScheduleExceptionByUserIdQuery(
    Guid UserId,
    Guid ScheduleExceptionId) : IRequest<Result<ScheduleExceptionDto>>;