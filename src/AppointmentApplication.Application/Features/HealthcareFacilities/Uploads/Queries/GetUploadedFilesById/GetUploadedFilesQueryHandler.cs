using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Dtos;
using AppointmentApplication.Application.HealthcareFacilities.Patients.Uploads.Mappers;

using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Commands.ChangeFileToPrivate
{
    public class GetUploadedFilesQueryHandler : IRequestHandler<GetUploadedFilesQuery, Result<List<UploadDto>>>
    {
        private readonly IAppDbContext _context;
        public GetUploadedFilesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<UploadDto>>> Handle(GetUploadedFilesQuery request, CancellationToken cancellationToken)
        {
            var healthCareFacility = await _context.HealthcareFacilities
                .Include(p => p.Uploads)
                .FirstOrDefaultAsync(p => p.Id == request.HealthCareFacilityId);
            if (healthCareFacility == null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.HealthCareFacilityId);
            }
            var uploads = healthCareFacility.Uploads.ToDtos();
            return uploads;
        }
    }
}