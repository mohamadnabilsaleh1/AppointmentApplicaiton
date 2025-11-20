using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;
using AppointmentApplication.Application.Features.Doctors.Mappers;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Query;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Doctors;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.Queries.GetDoctorsByHealthCareFacilityId
{
    public class GetDoctorsByHealthCareFacilityIdQueryHandler : IRequestHandler<GetDoctorsByHealthCareFacilityIdQuery, Result<List<DoctorWithContactDto>>>
    {
        private readonly IAppDbContext _context;
        private readonly DynamicQueryService _dynamicQueryService;
        private readonly DataShapingService _dataShapingService;

        public GetDoctorsByHealthCareFacilityIdQueryHandler(
            IAppDbContext context,
            DataShapingService dataShapingService,
            DynamicQueryService dynamicQueryService)
        {
            _context = context;
            _dataShapingService = dataShapingService;
            _dynamicQueryService = dynamicQueryService;
        }

        public async Task<Result<List<DoctorWithContactDto>>> Handle(GetDoctorsByHealthCareFacilityIdQuery request, CancellationToken cancellationToken)
        {
            // ✅ Direct query with all necessary includes for better performance
            var doctors = await _context.Doctors
                .Where(d => d.FacilityId == request.HealthCareFacilityId)
                .Include(d => d.User)
                    .ThenInclude(u => u.Emails)
                .Include(d => d.User)
                    .ThenInclude(u => u.Phones)
                .Include(d => d.Reviews) // ✅ Include reviews for statistics
                .Include(d => d.HealthcareFacility)
                .ToListAsync(cancellationToken);

            if (!doctors.Any())
            {
                // ✅ Check if facility exists even if no doctors
                var facilityExists = await _context.HealthcareFacilities
                    .AnyAsync(h => h.Id == request.HealthCareFacilityId, cancellationToken);
                
                if (!facilityExists)
                {
                    return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.HealthCareFacilityId);
                }
                
                // Return empty list if facility exists but has no doctors
                return new List<DoctorWithContactDto>();
            }

            // ✅ Return DoctorWithContactDto which includes review statistics
            return doctors.ToDtosWithContact();
        }
    }
}