using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Queries.GetSchedulesByIdQuery
{
    public class GetSchedulesByIdQueryHandler : IRequestHandler<GetSchedulesByIdQuery, Result<List<ScheduleDto>>>
    {
        private readonly IAppDbContext _context;

        public GetSchedulesByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<ScheduleDto>>> Handle(GetSchedulesByIdQuery request, CancellationToken cancellationToken)
        {
            var facility = await _context.HealthcareFacilities
                .Include(f => f.Schedules)
                .FirstOrDefaultAsync(f => f.Id == request.HealthCareFacilityId, cancellationToken);

            if (facility is null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.HealthCareFacilityId);
            }

            var schedules = facility.Schedules.ToDtos();

            return schedules;
        }
    }
}