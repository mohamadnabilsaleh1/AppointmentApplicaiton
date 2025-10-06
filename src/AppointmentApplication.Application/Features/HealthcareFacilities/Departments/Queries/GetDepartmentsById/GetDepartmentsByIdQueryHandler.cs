using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Queries.GetDepartmentsById
{
    public class GetDepartmentsByIdQueryHandler: IRequestHandler<GetDepartmentsByIdQuery, Result<List<DepartmentDto>>>
    {
        private readonly IAppDbContext _context;

        public GetDepartmentsByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<DepartmentDto>>> Handle(GetDepartmentsByIdQuery request, CancellationToken cancellationToken)
        {
            var facility = await _context.HealthcareFacilities
            .Include(f => f.Departments)
            .FirstOrDefaultAsync(f => f.Id == request.HealthCareFacilityId, cancellationToken);

            if (facility is null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.HealthCareFacilityId);
            }

            var departments = facility.Departments.ToDtos();

            return departments;
        }
    }
}