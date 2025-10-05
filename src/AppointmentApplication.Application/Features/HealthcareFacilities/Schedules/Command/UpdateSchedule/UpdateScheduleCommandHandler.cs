// 📁 Application/HealthcareFacilities/Schedules/Commands/UpdateScheduleCommandHandler.cs
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Mappers;
using AppointmentApplication.Application.HealthcareFacilities.Schedules.Commands;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Commands;

public class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;

    public UpdateScheduleCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Updated>> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
    {
        var facility = await _context.HealthcareFacilities
            .Include(f => f.Schedules)
            .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

        if (facility is null)
        {
            return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
        }

        var updateResult = facility.UpdateSchedule(
            request.ScheduleId,
            request.DayOfWeek,
            request.StartTime,
            request.EndTime,
            request.Status,
            request.IsAvailable,
            request.Note);

        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}