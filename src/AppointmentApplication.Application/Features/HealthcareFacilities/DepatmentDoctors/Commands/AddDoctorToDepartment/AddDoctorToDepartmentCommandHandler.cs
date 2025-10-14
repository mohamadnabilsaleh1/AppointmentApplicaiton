using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Errors;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Doctors;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.DepatmentDoctors.Commands.AddDoctorToDepartment
{
    public class AddDoctorToDepartmentCommandHandler : IRequestHandler<AddDoctorToDepartmentCommand, Result<Success>>
    {
        private readonly IAppDbContext _context;
        public AddDoctorToDepartmentCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<Success>> Handle(AddDoctorToDepartmentCommand request, CancellationToken cancellationToken)
        {
            var facility = await _context.HealthcareFacilities
                .Include(h => h.Departments)
                    .ThenInclude(d => d.Doctors)
                .Include(h => h.Doctors) // Include doctors for each department
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
            var doctor = facility.GetDoctorById(request.DoctorId);
            if(doctor.IsError)
            {
                return doctor.Errors;
            }
            var department = departmentResult.Value;
            var doctorResult = department.AddDoctor(doctor.Value);
            if (doctorResult.IsError)
            {
                return doctorResult.Errors;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success;
        }

    }
}