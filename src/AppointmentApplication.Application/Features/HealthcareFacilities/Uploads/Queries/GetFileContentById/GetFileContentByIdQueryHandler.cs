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
    public class GetFileContentByIdQueryHandler : IRequestHandler<GetFileContentByIdQuery, Result<FileContentResult>>
    {
        private readonly IAppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public GetFileContentByIdQueryHandler(IAppDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<FileContentResult>> Handle(GetFileContentByIdQuery request, CancellationToken cancellationToken)
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

            // Extract the relative path from fileURL
            var relativeFilePath = "/home/kali/Desktop/AppointmentApplication/Uploads/healthCareFacility/08ab65af-cdb4-4f2f-910f-cd45a1ba5eaf/uploads/bb6538a4-4bf1-4d94-b612-36a9c8738afa.png";
            // Get the file bytes from storage
            var fileBytes = await _fileStorageService.GetFileAsync(relativeFilePath);

            // Determine content type - use stored file type or detect from filename
            var contentType = upload.FileType;

            return new FileContentResult(fileBytes, contentType, upload.FacilityId.ToString());
        }

        private string GetRelativeFilePathFromUrl(string fileUrl)
        {
            // Convert from: "/api/files/healthCareFacility/08ab65af-cdb4-4f2f-910f-cd45a1ba5eaf/uploads/bb6538a4-4bf1-4d94-b612-36a9c8738afa.png"
            // To: "healthCareFacility/08ab65af-cdb4-4f2f-910f-cd45a1ba5eaf/uploads/bb6538a4-4bf1-4d94-b612-36a9c8738afa.png"
            if (string.IsNullOrEmpty(fileUrl))
                return string.Empty;

            // Remove the API route prefix
            var prefix = "/api/files/";
            if (fileUrl.StartsWith(prefix))
            {
                // Get the part after "/api/files/"
                var pathAfterApi = fileUrl.Substring(prefix.Length);
                return pathAfterApi;
            }

            // If it doesn't start with the expected prefix, try to extract the last parts
            var segments = fileUrl.Split('/');
            if (segments.Length >= 4)
            {
                // Take the last segments: healthCareFacility/{facilityId}/uploads/{filename}
                return string.Join("/", segments[^4..]);
            }

            return fileUrl;
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".txt" => "text/plain",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream"
            };
        }
    }
}