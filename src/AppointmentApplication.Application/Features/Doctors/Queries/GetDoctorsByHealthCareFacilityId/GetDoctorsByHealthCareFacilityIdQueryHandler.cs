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

    public class GetDoctorsByHealthCareFacilityIdQueryHandler : IRequestHandler<GetDoctorsByHealthCareFacilityIdQuery, Result<List<DoctorDto>>>
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

        public async Task<Result<List<DoctorDto>>> Handle(GetDoctorsByHealthCareFacilityIdQuery request, CancellationToken cancellationToken)
        {
            var healthcareFacility = await _context.HealthcareFacilities
                .Include(h => h.Doctors)
                .FirstOrDefaultAsync(h => h.Id == request.HealthCareFacilityId, cancellationToken);

            if (healthcareFacility is null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.HealthCareFacilityId);
            }

            var doctors = healthcareFacility.Doctors;

            return doctors.ToDtos();
        }
    }
}