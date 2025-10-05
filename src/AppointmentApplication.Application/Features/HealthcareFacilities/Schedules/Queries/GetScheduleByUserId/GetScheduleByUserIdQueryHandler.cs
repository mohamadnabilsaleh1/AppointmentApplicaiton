// 📁 Application/HealthcareFacilities/Schedules/Queries/GetScheduleByIdQueryHandler.cs
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

public class GetScheduleByUserIdQueryHandler : IRequestHandler<GetScheduleByUserIdQuery, Result<ScheduleDto>>
{
    private readonly IAppDbContext _context;

    public GetScheduleByUserIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ScheduleDto>> Handle(GetScheduleByUserIdQuery request, CancellationToken cancellationToken)
    {
        var facility = await _context.HealthcareFacilities
            .Include(f => f.Schedules)
            .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

        if (facility is null)
        {
            return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
        }

        var scheduleResult = facility.GetScheduleById(request.ScheduleId);
        if (scheduleResult.IsError)
        {
            return scheduleResult.Errors;
        }

        return scheduleResult.Value.ToDto();
    }
}