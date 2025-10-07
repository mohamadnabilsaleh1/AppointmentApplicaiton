using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;

using AppointmentApplication.Application.Features.Doctors.Errors;
using AppointmentApplication.Application.Features.Doctors.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Query;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.Queries.GetDoctorByUserId
{
    public class GetDoctorByUserIdQueryHandler : IRequestHandler<GetDoctorByUserIdQuery, Result<DoctorDto>>
    {
        private readonly IAppDbContext _context;
        private readonly DynamicQueryService _dynamicQueryService;
        private readonly DataShapingService _dataShapingService;

        public GetDoctorByUserIdQueryHandler(
            IAppDbContext context,
            DataShapingService dataShapingService,
            DynamicQueryService dynamicQueryService)
        {
            _context = context;
            _dataShapingService = dataShapingService;
            _dynamicQueryService = dynamicQueryService;
        }
        public async Task<Result<DoctorDto>> Handle(GetDoctorByUserIdQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == request.UserId);
            if (doctor is null)
            {
                return ApplicationDoctorErrors.DoctorNotFound(request.UserId);
            }

            return doctor.ToDto();
        }
    }
}