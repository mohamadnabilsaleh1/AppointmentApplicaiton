// 📁 Application/HealthcareFacilities/ScheduleExceptions/Queries/GetScheduleExceptionByIdQuery.cs
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.ScheduleExceptions.Mappers;

public sealed record GetScheduleExceptionByIdQuery(
    Guid DoctorId,
    Guid ScheduleExceptionId) : IRequest<Result<ScheduleExceptionDto>>;