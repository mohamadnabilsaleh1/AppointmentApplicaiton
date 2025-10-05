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

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Commands
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Result<DepartmentDto>>
    {
        private readonly IAppDbContext _context;
        public CreateDepartmentCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<DepartmentDto>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var facility = await _context.HealthcareFacilities
                .Include(f => f.Departments)
                .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

            if (facility is null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
            }

            var departmentResult = facility.AddDepartment(request.Name, request.Description);
            if (departmentResult.IsError)
            {
                return departmentResult.Errors;

            }

            await _context.SaveChangesAsync(cancellationToken);
            return departmentResult.Value.ToDto();

        }
    }
}