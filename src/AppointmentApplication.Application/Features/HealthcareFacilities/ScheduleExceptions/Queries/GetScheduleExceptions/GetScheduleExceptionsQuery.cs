// 📁 Application/HealthcareFacilities/ScheduleExceptions/Queries/GetAllScheduleExceptionsQuery.cs
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Queries;

public sealed record GetScheduleExceptionsQuery(
    Guid UserId,
    DateOnly? StartDate = null,
    DateOnly? EndDate = null) : IRequest<Result<List<ScheduleExceptionDto>>>;