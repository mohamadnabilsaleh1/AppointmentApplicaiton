// 📁 Application/HealthcareFacilities/Schedules/Commands/DeleteScheduleCommandHandler.cs
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.HealthcareFacilities.Schedules.Commands;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.Schedules.Commands;

public class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, Result<Deleted>>
{
    private readonly IAppDbContext _context;

    public DeleteScheduleCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Deleted>> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
        .Include(f => f.Schedules)
            .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

        if (doctor is null)
        {
            return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
        }

        var deleteResult = doctor.DeleteSchedule(request.ScheduleId);
        if (deleteResult.IsError)
        {
            return deleteResult.Errors;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}