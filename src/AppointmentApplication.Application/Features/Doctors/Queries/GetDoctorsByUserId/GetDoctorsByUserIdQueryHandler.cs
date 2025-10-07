using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;
using AppointmentApplication.Application.Features.Doctors.Mappers;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Query;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.Queries.GetDoctorByUserId
{
    public class GetDoctorsByUserIdQueryHandler : IRequestHandler<GetDoctorsByUserIdQuery, Result<List<DoctorDto>>>
    {
        private readonly IAppDbContext _context;
        private readonly DynamicQueryService _dynamicQueryService;
        private readonly DataShapingService _dataShapingService;

        public GetDoctorsByUserIdQueryHandler(
            IAppDbContext context,
            DataShapingService dataShapingService,
            DynamicQueryService dynamicQueryService)
        {
            _context = context;
            _dataShapingService = dataShapingService;
            _dynamicQueryService = dynamicQueryService;
        }

        public async Task<Result<List<DoctorDto>>> Handle(GetDoctorsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var healthcareFacility = await _context.HealthcareFacilities
                .Include(h => h.Doctors)
                .FirstOrDefaultAsync(h => h.UserId == request.UserId, cancellationToken);

            if (healthcareFacility is null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
            }

            var doctors = healthcareFacility.Doctors;

            return doctors.ToDtos();
        }

    }
}