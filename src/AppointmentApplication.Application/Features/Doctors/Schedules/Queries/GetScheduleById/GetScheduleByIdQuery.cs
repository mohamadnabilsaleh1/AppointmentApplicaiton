// 📁 Application/HealthcareFacilities/Schedules/Queries/GetScheduleByIdQuery.cs
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.Schedules.Queries;

public sealed record GetScheduleByIdQuery(
    Guid DoctorId,
    Guid ScheduleId) : IRequest<Result<ScheduleDto>>;