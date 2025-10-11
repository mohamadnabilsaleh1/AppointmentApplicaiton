using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.MediaUploads;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Patients.Uploads.Commands.CreateUploadFile
{
    public class CreateUploadFileCommandHandler : IRequestHandler<CreateUploadFileCommand, Result<PatientUpload>>
    {
        private readonly ILogger<CreateUploadFileCommandHandler> _logger;
        private readonly IAppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public CreateUploadFileCommandHandler(
            ILogger<CreateUploadFileCommandHandler> logger,
            IAppDbContext context,
            IFileStorageService fileStorageService)
        {
            _logger = logger;
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<PatientUpload>> Handle(CreateUploadFileCommand request, CancellationToken cancellationToken)
        {
            var patient = _context.Patients
                .FirstOrDefault(p => p.UserId == request.UserId);

            if (patient is null)
            {
                _logger.LogWarning("Patient not found. ID: {PatientId}", request.UserId);
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }

            // Save the file and get the file name/path
            var fileName = await _fileStorageService.SaveFileAsync(
                request.File,
                $"patients/{patient.Id}/uploads");

            // Create the file URL (you might want to configure this base URL)
            var fileUrl = $"/api/files/patients/{patient.Id}/uploads/{fileName}";

            var uploadResult = patient.AddUpload(
                patient.Id,
                request.File.ContentType,
                fileUrl, // Use the actual file URL
                request.Title,
                request.Description,
                request.Visibility);

            if (uploadResult.IsError)
            {
                _logger.LogWarning("Failed to create upload: {Error}", uploadResult.Errors);
                return uploadResult.Errors;
            }

            // Add to context and save changes
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "File uploaded successfully for patient {PatientId}. Upload ID: {UploadId}",
                patient.Id, uploadResult.Value.Id);

            return uploadResult.Value;

        }
    }
}