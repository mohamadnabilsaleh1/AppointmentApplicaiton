// 📁 Application/HealthcareFacilities/Schedules/Queries/GetAllSchedulesQueryHandler.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Mappers;
using AppointmentApplication.Application.HealthcareFacilities.Schedules.Queries;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Queries;

public class GetSchedulesQueryHandler : IRequestHandler<GetSchedulesQuery, Result<List<ScheduleDto>>>
{
    private readonly IAppDbContext _context;

    public GetSchedulesQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ScheduleDto>>> Handle(GetSchedulesQuery request, CancellationToken cancellationToken)
    {
        var facility = await _context.HealthcareFacilities
            .Include(f => f.Schedules)
            .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

        if (facility is null)
        {
            return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
        }

        var schedules = facility.Schedules
            .Select(schedule => schedule.ToDto())
            .ToList();
            
        return schedules;
    }
}