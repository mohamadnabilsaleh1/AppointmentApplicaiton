using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Result<Updated>>
    {
        private readonly IAppDbContext _context;
        public UpdateDepartmentCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Updated>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var facility = await _context.HealthcareFacilities
            .Include(f => f.Departments)
                .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

            if (facility is null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
            }

            var updateResult = facility.UpdateDepartment(request.DepartmentId, request.Name, request.Description);
            if (updateResult.IsError)
            {
                return updateResult.Errors;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Updated;
        }
    }
}