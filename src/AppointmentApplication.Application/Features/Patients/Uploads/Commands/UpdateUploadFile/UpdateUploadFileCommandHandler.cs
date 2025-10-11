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

namespace AppointmentApplication.Application.Features.Patients.Uploads.Commands.UpdateUploadFile
{
    public class UpdateUploadFileCommandHandler : IRequestHandler<UpdateUploadFileCommand, Result<Updated>>
    {
        private readonly ILogger<UpdateUploadFileCommandHandler> _logger;
        private readonly IAppDbContext _context;
        public UpdateUploadFileCommandHandler(
            ILogger<UpdateUploadFileCommandHandler> logger,
            IAppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<Result<Updated>> Handle(UpdateUploadFileCommand request, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients
            .Include(p => p.Uploads)
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);
            if (patient is null)
            {
                _logger.LogWarning("Patient not found. ID: {PatientId}", request.UserId);
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }
            var upload = patient.UpdateUpload(request.UploadId, request.Title, request.Description);
            if (upload.IsError)
            {
                _logger.LogWarning("Upload not found. Upload ID: {UploadId}", request.UploadId);
                return upload.Errors;
            }
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Updated;
        }

    }
}