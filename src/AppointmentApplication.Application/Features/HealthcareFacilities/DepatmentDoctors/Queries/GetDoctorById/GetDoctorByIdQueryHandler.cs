// 📁 Application/HealthcareFacilities/Schedules/Queries/GetScheduleByIdQueryHandler.cs
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;
using AppointmentApplication.Application.Features.Doctors.Mappers;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Mappers;
using AppointmentApplication.Application.HealthcareFacilities.Schedules.Queries;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Queries;

public class GetDoctorByIdQueryHandler : IRequestHandler<GetDoctorByIdQuery, Result<DepartmentDoctorsDto>>
{
    private readonly IAppDbContext _context;

    public GetDoctorByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DepartmentDoctorsDto>> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
    {
        var facility = await _context.HealthcareFacilities
                .Include(f => f.Departments)
                    .ThenInclude(d => d.Doctors)
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
        var department = departmentResult.Value;
        var doctorResult = department.GetDoctor(request.DoctorId);
        if (doctorResult.IsError)
        {
            return doctorResult.Errors;
        }
        var doctor = doctorResult.Value;
        //entities.Select(e => e.DepartmentDoctorsToDto()).ToList();
        return doctor.DepartmentDoctorsToDto();
    }
}