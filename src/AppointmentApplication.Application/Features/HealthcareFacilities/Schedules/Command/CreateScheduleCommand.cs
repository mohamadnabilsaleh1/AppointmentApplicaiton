// 📁 Application/HealthcareFacilities/Schedules/Commands/CreateScheduleCommand.cs
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.HealthcareFacilities.Schedules.Commands;

public sealed record CreateScheduleCommand(
    Guid FacilityId,
    DaysOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    Status Status,
    string? Note = null) : IRequest<Result<ScheduleDto>>;