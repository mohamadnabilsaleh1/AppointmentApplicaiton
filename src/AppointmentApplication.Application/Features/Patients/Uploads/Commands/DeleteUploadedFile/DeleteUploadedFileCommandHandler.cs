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

namespace AppointmentApplication.Application.Features.Patients.Uploads.Commands.DeleteUploadedFile
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
            var patient = await _context.Patients
            .Include(p => p.Uploads)
            .FirstOrDefaultAsync(p => p.UserId == request.UserId);
            if (patient is null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }
            var uploadResult = patient.DeleteUploadedFile(request.FileId);
            if (uploadResult.IsError)
            {
                return uploadResult.Errors;
            }
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }

    }
}