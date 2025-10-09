using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.ScheduleExceptions.Mappers
{
    public sealed class GetScheduleExceptionsByIdQueryHandler : IRequestHandler<GetScheduleExceptionsByIdQuery, Result<List<ScheduleExceptionDto>>>
    {
        private readonly IAppDbContext _context;

        public GetScheduleExceptionsByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<ScheduleExceptionDto>>> Handle(GetScheduleExceptionsByIdQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _context.Doctors
                .Include(f => f.ScheduleExceptions)
                .FirstOrDefaultAsync(f => f.Id == request.DoctorId, cancellationToken);

            if (doctor is null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.DoctorId);
            }

            var exceptions = doctor.ScheduleExceptions.AsQueryable();

            if (request.StartDate.HasValue)
            {
                exceptions = exceptions.Where(e => e.Date >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                exceptions = exceptions.Where(e => e.Date <= request.EndDate.Value);
            }

            var result = exceptions
                .OrderBy(e => e.Date)
                .ThenBy(e => e.StartTime)
                .Select(exception => exception.ToDto())
                .ToList();

            return result; // ✅ fixed line
        }
    }
}