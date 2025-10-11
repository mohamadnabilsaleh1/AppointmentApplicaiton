using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Errors;

using AppointmentApplication.Application.Features.Patients.Uploads.Dtos;
using AppointmentApplication.Application.Features.Patients.Uploads.Mappers;

using AppointmentApplication.Application.Features.Patients.Uploads.Queries.GetUploadedFileByUserIdQuery;
using AppointmentApplication.Application.HealthcareFacilities.Patients.Uploads.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Commands.ChangeFileToPrivate
{
    public class GetUploadedFilesByUserIdQueryHandler : IRequestHandler<GetUploadedFilesByUserIdQuery, Result<List<UploadDto>>>
    {
        private readonly IAppDbContext _context;
        public GetUploadedFilesByUserIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<UploadDto>>> Handle(GetUploadedFilesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var healthCareFacility = await _context.HealthcareFacilities
            .Include(p => p.Uploads)
            .FirstOrDefaultAsync(p => p.UserId == request.UserId);
            if (healthCareFacility is null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }
            var uploads = healthCareFacility.Uploads.ToDtos();
            return uploads;
        }

    }
}