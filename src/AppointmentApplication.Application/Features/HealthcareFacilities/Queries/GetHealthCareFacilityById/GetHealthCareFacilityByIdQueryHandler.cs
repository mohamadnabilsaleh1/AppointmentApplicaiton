using System;
using System.Threading;
using System.Threading.Tasks;
using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilityById
{
    public sealed class GetHealthCareFacilityByIdQueryHandler
        : IRequestHandler<GetHealthCareFacilityByIdQuery, Result<HealthcareFacilityDto>>
    {
        private readonly IAppDbContext _context;

        public GetHealthCareFacilityByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<HealthcareFacilityDto>> Handle(
            GetHealthCareFacilityByIdQuery request,
            CancellationToken cancellationToken)
        {
            var facility = await _context.HealthcareFacilities
                .Include(f => f.Departments)
                .Include(f => f.Schedules)
                .Include(f => f.ScheduleExceptions)
                .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

            if (facility is null)
            {

                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.Id);
            }
            if (!facility.IsActive) // assuming your entity has IsActive
            {

                return ApplicationHealthCareFacilityErrors.FacilityInactive(facility.Id);
            }
            return facility.ToDto();
        }
    }
}
