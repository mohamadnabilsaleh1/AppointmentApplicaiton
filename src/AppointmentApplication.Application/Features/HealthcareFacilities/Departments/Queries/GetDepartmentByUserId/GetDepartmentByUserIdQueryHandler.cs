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

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Queries.GetDepartmentByUserId
{
    public class GetDepartmentByUserIdQueryHandler : IRequestHandler<GetDepartmentByUserIdQuery, Result<DepartmentDto>>
    {
        private readonly IAppDbContext _context;

        public GetDepartmentByUserIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<DepartmentDto>> Handle(GetDepartmentByUserIdQuery request, CancellationToken cancellationToken)
        {
            var facility = await _context.HealthcareFacilities
            .Include(d => d.Departments)
            .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

            if (facility is null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
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