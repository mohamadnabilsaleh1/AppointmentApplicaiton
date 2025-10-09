// 📁 Application/HealthcareFacilities/ScheduleExceptions/Queries/GetAllScheduleExceptionsQuery.cs
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.ScheduleExceptions.Mappers;

public sealed record GetScheduleExceptionsByUserIdQuery(
    Guid UserId,
    DateOnly? StartDate = null,
    DateOnly? EndDate = null) : IRequest<Result<List<ScheduleExceptionDto>>>;