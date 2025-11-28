using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;
using AppointmentApplication.Application.Features.Doctors.Mappers;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Queries.GetSchedulesByIdQuery
{
    public class GetTopDoctorsByDepartmentQueryHandler
        : IRequestHandler<GetTopDoctorsByDepartmentQuery, Result<List<DepartmentDoctorsDto>>>
    {
        private readonly IAppDbContext _context;

        public GetTopDoctorsByDepartmentQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<DepartmentDoctorsDto>>> Handle(
            GetTopDoctorsByDepartmentQuery request,
            CancellationToken cancellationToken)
        {
            var facility = await _context.HealthcareFacilities
                .Include(f => f.Departments)
                    .ThenInclude(d => d.Doctors)
                        .ThenInclude(doc => doc.Reviews)
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
            var doctors = department.Doctors.ToList();

            var limit = request.Limit <= 0 ? 5 : request.Limit;

            var topDoctors = doctors
                .Select(doc =>
                {
                    var totalReviews = doc.Reviews?.Count ?? 0;
                    var averageRating = totalReviews > 0
                        ? Math.Round(doc.Reviews.Average(r => r.Rating), 1)
                        : 0;

                    return new
                    {
                        Doctor = doc,
                        AverageRating = averageRating,
                        TotalReviews = totalReviews
                    };
                })
                .OrderByDescending(d => d.AverageRating)
                .ThenByDescending(d => d.TotalReviews)
                .ThenBy(d => d.Doctor.LastName)
                .ThenBy(d => d.Doctor.FirstName)
                .Take(limit)
                .Select(d => d.Doctor.DepartmentDoctorsToDto())
                .ToList();

            return topDoctors;
        }
    }
}
