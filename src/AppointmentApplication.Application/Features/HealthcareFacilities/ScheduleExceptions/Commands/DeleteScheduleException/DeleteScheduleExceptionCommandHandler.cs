// 📁 Application/HealthcareFacilities/ScheduleExceptions/Commands/DeleteScheduleExceptionCommandHandler.cs
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Commands;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Commands;

public class DeleteScheduleExceptionCommandHandler : IRequestHandler<DeleteScheduleExceptionCommand, Result<Deleted>>
{
    private readonly IAppDbContext _context;

    public DeleteScheduleExceptionCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Deleted>> Handle(DeleteScheduleExceptionCommand request, CancellationToken cancellationToken)
    {
        var facility = await _context.HealthcareFacilities
            .Include(f => f.ScheduleExceptions)
            .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

        if (facility is null)
        {
            return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
        }

        var deleteResult = facility.DeleteScheduleException(request.ExceptionId);
        if (deleteResult.IsError)
        {
            return deleteResult.Errors;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}