// 📁 Application/HealthcareFacilities/Schedules/Queries/GetScheduleByIdQuery.cs
using AppointmentApplication.Application.Features.Doctors.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.HealthcareFacilities.Schedules.Queries;

public sealed record GetDoctorByUserIdQuery(
    Guid UserId,
    Guid DepartmentId, Guid DoctorId) : IRequest<Result<DepartmentDoctorsDto>>;