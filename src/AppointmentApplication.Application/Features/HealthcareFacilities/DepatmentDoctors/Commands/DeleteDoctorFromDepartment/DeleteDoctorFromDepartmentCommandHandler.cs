using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.DepatmentDoctors.Commands.DeleteDoctorFromDepartment
{
    public class DeleteDoctorFromDepartmentCommandHandler : IRequestHandler<DeleteDoctorFromDepartmentCommand, Result<Deleted>>
    {
        private readonly IAppDbContext _context;
        public DeleteDoctorFromDepartmentCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<Deleted>> Handle(DeleteDoctorFromDepartmentCommand request, CancellationToken cancellationToken)
        {
            var facility = await _context.HealthcareFacilities
            .Include(h => h.Departments)
            .ThenInclude(d => d.Doctors)
            .FirstOrDefaultAsync(h => h.UserId == request.UserId);
            if (facility is null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
            }

            var departmentResult = facility.GetDepartmentById(request.DepartmentId);
            if (departmentResult.IsError)
            {
                return departmentResult.Errors;
            }

            var department = departmentResult.Value;
            var doctorResult = department.RemoveDoctor(request.DoctorId);
            if (doctorResult.IsError)
            {
                return doctorResult.Errors;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }
    }
}