// 📁 Application/HealthcareFacilities/ScheduleExceptions/Commands/CreateScheduleExceptionCommandHandler.cs
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Mappers;
using AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Commands;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Commands;

public class CreateScheduleExceptionCommandHandler : IRequestHandler<CreateScheduleExceptionCommand, Result<ScheduleExceptionDto>>
{
    private readonly IAppDbContext _context;

    public CreateScheduleExceptionCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ScheduleExceptionDto>> Handle(CreateScheduleExceptionCommand request, CancellationToken cancellationToken)
    {
        var facility = await _context.HealthcareFacilities
            .Include(f => f.ScheduleExceptions)
            .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

        if (facility is null)
        {
            return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
        }

        var exceptionResult = facility.AddScheduleException(
            request.Date,
            request.StartTime,
            request.EndTime,
            request.Status,
            request.Reason);

        if (exceptionResult.IsError)
        {
            return exceptionResult.Errors;
        }

        _context.HealthcareFacilityScheduleExceptions.Add(exceptionResult.Value);
        await _context.SaveChangesAsync(cancellationToken);

        return exceptionResult.Value.ToDto();
    }
}