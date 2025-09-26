using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilityById
{
    public sealed class GetHealthCareFacilityByIdQueryHandler
        : IRequestHandler<GetHealthCareFacilityByIdQuery, Result<ExpandoObject>>
    {
        private readonly IAppDbContext _context;
        private readonly DataShapingService _dataShapingService;

        public GetHealthCareFacilityByIdQueryHandler(
            DataShapingService dataShapingService,
            IAppDbContext context)
        {
            _context = context;
            _dataShapingService = dataShapingService;
        }

        public async Task<Result<ExpandoObject>> Handle(GetHealthCareFacilityByIdQuery request, CancellationToken cancellationToken)
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

            // Map entity to DTO
            var dto = facility.ToDto();

            // Shape DTO into ExpandoObject (only requested fields)
            ExpandoObject shapedItem = _dataShapingService.ShapeData(dto, request.Fields);

            // Implicit conversion: ExpandoObject -> Result<ExpandoObject>
            return shapedItem;
        }
    }
}
