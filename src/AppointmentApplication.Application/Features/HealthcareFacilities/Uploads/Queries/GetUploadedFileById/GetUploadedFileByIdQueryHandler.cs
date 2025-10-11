using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Errors;

using AppointmentApplication.Application.Features.Patients.Uploads.Dtos;
using AppointmentApplication.Application.Features.Patients.Uploads.Mappers;
using AppointmentApplication.Application.HealthcareFacilities.Patients.Uploads.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Commands.ChangeFileToPrivate
{
    public class GetUploadedFileByIdQueryHandler : IRequestHandler<GetUploadedFileByIdQuery, Result<UploadDto>>
    {
        private readonly IAppDbContext _context;
        public GetUploadedFileByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<UploadDto>> Handle(GetUploadedFileByIdQuery request, CancellationToken cancellationToken)
        {
            var healthCareFacility = await _context.HealthcareFacilities
                .Include(p => p.Uploads)
                .FirstOrDefaultAsync(p => p.Id == request.PatientId);
            if (healthCareFacility == null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.PatientId);
            }
            var uploadResult = healthCareFacility.GetUploadedById(request.FileId);
            if (uploadResult.IsError)
            {
                return uploadResult.Errors;
            }
            return uploadResult.Value.ToDto();
        }

    }

}