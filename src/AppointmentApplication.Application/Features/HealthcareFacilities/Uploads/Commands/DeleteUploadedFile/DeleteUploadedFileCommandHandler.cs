using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Errors;

using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Commands.ChangeFileToPrivate
{
    public class DeleteUploadedFileCommandHandler : IRequestHandler<DeleteUploadedFileCommand, Result<Deleted>>
    {
        private readonly ILogger<DeleteUploadedFileCommandHandler> _logger;
        private readonly IAppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public DeleteUploadedFileCommandHandler(
            ILogger<DeleteUploadedFileCommandHandler> logger,
            IAppDbContext context,
            IFileStorageService fileStorageService)
        {
            _logger = logger;
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<Deleted>> Handle(DeleteUploadedFileCommand request, CancellationToken cancellationToken)
        {
            var healthCareFacility = await _context.HealthcareFacilities
            .Include(p => p.Uploads)
            .FirstOrDefaultAsync(p => p.UserId == request.UserId);
            if (healthCareFacility is null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }
            var uploadResult = healthCareFacility.DeleteUploadedFile(request.FileId);
            if (uploadResult.IsError)
            {
                return uploadResult.Errors;
            }
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }

    }
}