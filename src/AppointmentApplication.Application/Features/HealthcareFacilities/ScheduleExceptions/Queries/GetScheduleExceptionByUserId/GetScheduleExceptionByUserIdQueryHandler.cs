using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Mappers;

using AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Queries;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Queries.GetScheduleExceptionByUserId
{
    public class GetScheduleExceptionByUserIdQueryHandler : IRequestHandler<GetScheduleExceptionByUserIdQuery, Result<ScheduleExceptionDto>>
    {
        private readonly IAppDbContext _context;

        public GetScheduleExceptionByUserIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ScheduleExceptionDto>> Handle(GetScheduleExceptionByUserIdQuery request, CancellationToken cancellationToken)
        {
            var facility = await _context.HealthcareFacilities
                 .Include(f => f.ScheduleExceptions)
                 .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

            if (facility is null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
            }

            var exceptionResult = facility.GetScheduleExceptionById(request.ScheduleExceptionId);
            if (exceptionResult.IsError)
            {
                return exceptionResult.Errors;
            }

            return exceptionResult.Value.ToDto();
        }

    }
}