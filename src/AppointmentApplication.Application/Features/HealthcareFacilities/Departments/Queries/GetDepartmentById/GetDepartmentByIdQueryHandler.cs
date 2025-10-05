using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Mappers;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, Result<DepartmentDto>>
    {

        private readonly IAppDbContext _context;

        public GetDepartmentByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<DepartmentDto>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var facility = await _context.HealthcareFacilities
            .Include(d => d.Departments)
            .FirstOrDefaultAsync(f => f.Id == request.HealthCareFacilityId, cancellationToken);

            if (facility is null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.HealthCareFacilityId);
            }

            var departmentResult = facility.GetDepartmentById(request.DepartmentId);
            if (departmentResult.IsError)
            {
                return departmentResult.Errors;
            }

            return departmentResult.Value.ToDto();
        }
    }
}
