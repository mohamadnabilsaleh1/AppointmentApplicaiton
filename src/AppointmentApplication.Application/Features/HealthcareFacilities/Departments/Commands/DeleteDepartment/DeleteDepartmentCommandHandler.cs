using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, Result<Deleted>>
    {
        private readonly IAppDbContext _context;

        public DeleteDepartmentCommandHandler(IAppDbContext context) {
            _context = context;
        }

        public async Task<Result<Deleted>> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var facility = await _context.HealthcareFacilities
            .Include(f =>f.Departments)
                .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

            if (facility is null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
            }

            var deleteResult = facility.DeleteDepartment(request.DepartmentId);
            if (deleteResult.IsError)
            {
                return deleteResult.Errors;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }
    }
}