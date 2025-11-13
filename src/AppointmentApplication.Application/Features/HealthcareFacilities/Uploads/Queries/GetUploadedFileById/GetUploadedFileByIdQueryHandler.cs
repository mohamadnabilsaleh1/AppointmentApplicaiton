using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Dtos;

using AppointmentApplication.Application.Features.Patients.Errors;
using AppointmentApplication.Application.HealthcareFacilities.Patients.Uploads.Mappers;

using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Commands.ChangeFileToPrivate
{
    public class GetUploadedFileByIdQueryHandler : IRequestHandler<GetUploadedFileByIdQuery, Result<FileUploadResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public GetUploadedFileByIdQueryHandler(IAppDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<FileUploadResponse>> Handle(GetUploadedFileByIdQuery request, CancellationToken cancellationToken)
        {
            var healthCareFacility = await _context.HealthcareFacilities
                .Include(p => p.Uploads)
                .FirstOrDefaultAsync(p => p.Id == request.HealthCareFacilityId);

            if (healthCareFacility == null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.HealthCareFacilityId);
            }

            var uploadResult = healthCareFacility.GetUploadedById(request.FileId);
            if (uploadResult.IsError)
            {
                return uploadResult.Errors;
            }

            var upload = uploadResult.Value;

            // Extract file name from fileURL
            var fileName = Path.GetFileName(upload.FileURL);

            // Get the actual file bytes
            var filePath = $"healthCareFacility/{healthCareFacility.Id}/uploads/{fileName}";
            var fileBytes = await _fileStorageService.GetFileAsync(filePath);


            // Return both the upload info and file content
            return new FileUploadResponse
            {
                Upload = upload.ToDto(),
                FileBytes = fileBytes,
                FileName = fileName,
                ContentType = upload.FileType
            };
        }
    }

}

