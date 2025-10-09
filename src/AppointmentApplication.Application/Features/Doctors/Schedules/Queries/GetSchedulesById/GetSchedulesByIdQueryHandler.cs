using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Errors;

using AppointmentApplication.Application.Features.Doctors.Schedules.Mapper;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.Schedules.Queries
{
    public class GetSchedulesByIdQueryHandler : IRequestHandler<GetSchedulesByIdQuery, Result<List<ScheduleDto>>>
    {
        private readonly IAppDbContext _context;

        public GetSchedulesByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<ScheduleDto>>> Handle(GetSchedulesByIdQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _context.Doctors
                .Include(f => f.Schedules)
                .FirstOrDefaultAsync(f => f.Id == request.DoctorId, cancellationToken);

            if (doctor is null)
            {
                return ApplicationDoctorErrors.DoctorNotFound(request.DoctorId);
            }

            var schedules = doctor.Schedules.ToDtos();

            return schedules;
        }
    }
}