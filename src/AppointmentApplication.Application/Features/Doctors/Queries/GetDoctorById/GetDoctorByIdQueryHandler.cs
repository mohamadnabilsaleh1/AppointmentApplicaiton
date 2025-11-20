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

namespace AppointmentApplication.Application.Features.Doctors.Queries.GetDoctorsById
{
    public class GetDoctorByIdQueryHandler : IRequestHandler<GetDoctorByIdQuery, Result<DoctorWithContactDto>>
    {
        private readonly IAppDbContext _context;
        private readonly DynamicQueryService _dynamicQueryService;
        private readonly DataShapingService _dataShapingService;

        public GetDoctorByIdQueryHandler(
            IAppDbContext context,
            DataShapingService dataShapingService,
            DynamicQueryService dynamicQueryService)
        {
            _context = context;
            _dataShapingService = dataShapingService;
            _dynamicQueryService = dynamicQueryService;
        }

        public async Task<Result<DoctorWithContactDto>> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            // ✅ Include all necessary data including Reviews
            var doctor = await _context.Doctors
                .Include(d => d.User)
                    .ThenInclude(u => u.Emails)
                .Include(d => d.User)
                    .ThenInclude(u => u.Phones)
                .Include(d => d.Reviews) // ✅ Include reviews for statistics
                .Include(d => d.HealthcareFacility)
                .FirstOrDefaultAsync(d => d.Id == request.DoctorId, cancellationToken);

            if (doctor is null)
            {
                return ApplicationDoctorErrors.DoctorNotFound(request.DoctorId);
            }

            // ✅ Return DoctorWithContactDto which includes review statistics
            return doctor.ToDtoWithContact();
        }
    }
}