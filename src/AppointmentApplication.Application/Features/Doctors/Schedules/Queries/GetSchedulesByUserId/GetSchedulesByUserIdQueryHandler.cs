// 📁 Application/HealthcareFacilities/Schedules/Queries/GetAllSchedulesQueryHandler.cs
using System.Collections.Generic;
using System.Linq;
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

public class GetSchedulesByUserIdQueryHandler : IRequestHandler<GetSchedulesByUserIdQuery, Result<List<ScheduleDto>>>
{
    private readonly IAppDbContext _context;

    public GetSchedulesByUserIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ScheduleDto>>> Handle(GetSchedulesByUserIdQuery request, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .Include(f => f.Schedules)
            .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

        if (doctor is null)
        {
            return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
        }

        var schedules = doctor.Schedules.ToDtos();

        return schedules;
    }
}