// 📁 Application/HealthcareFacilities/Schedules/Queries/GetScheduleByIdQueryHandler.cs
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Schedules.Mapper;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Mappers;
using AppointmentApplication.Application.HealthcareFacilities.Schedules.Queries;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.Schedules.Queries;

public class GetScheduleByIdQueryHandler : IRequestHandler<GetScheduleByIdQuery, Result<ScheduleDto>>
{
    private readonly IAppDbContext _context;

    public GetScheduleByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ScheduleDto>> Handle(GetScheduleByIdQuery request, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .Include(f => f.Schedules)
            .FirstOrDefaultAsync(f => f.Id == request.HealthCareFacilityId, cancellationToken);

        if (doctor is null)
        {
            return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.HealthCareFacilityId);
        }

        var scheduleResult = doctor.GetScheduleById(request.ScheduleId);
        if (scheduleResult.IsError)
        {
            return scheduleResult.Errors;
        }

        return scheduleResult.Value.ToDto();
    }
}