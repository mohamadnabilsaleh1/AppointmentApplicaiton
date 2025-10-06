// 📁 Application/HealthcareFacilities/ScheduleExceptions/Queries/GetAllScheduleExceptionsQueryHandler.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Mappers;
using AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Queries;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Queries;

public class GetScheduleExceptionsByUserIdQueryHandler : IRequestHandler<GetScheduleExceptionsByUserIdQuery, Result<List<ScheduleExceptionDto>>>
{
    private readonly IAppDbContext _context;

    public GetScheduleExceptionsByUserIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ScheduleExceptionDto>>> Handle(GetScheduleExceptionsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var facility = await _context.HealthcareFacilities
            .Include(f => f.ScheduleExceptions)
            .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

        if (facility is null)
        {
            return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
        }

        var exceptions = facility.ScheduleExceptions.AsQueryable();

        // Apply date filters if provided
        if (request.StartDate.HasValue)
        {
            exceptions = exceptions.Where(e => e.Date >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            exceptions = exceptions.Where(e => e.Date <= request.EndDate.Value);
        }

        var result = exceptions
            .OrderBy(e => e.Date)
            .ThenBy(e => e.StartTime)
            .Select(exception => exception.ToDto())
            .ToList();

        return result;
    }
}