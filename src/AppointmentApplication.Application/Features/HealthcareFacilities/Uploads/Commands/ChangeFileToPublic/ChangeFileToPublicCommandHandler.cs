using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Errors;

using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Commands.ChangeFileToPrivate
{
    public class ChangeFileToPublicCommandHandler : IRequestHandler<ChangeFileToPublicCommand, Result<Updated>>
    {
        private readonly IAppDbContext _context;
        public ChangeFileToPublicCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<Updated>> Handle(ChangeFileToPublicCommand request, CancellationToken cancellationToken)
        {
            var healthCareFacility = _context.HealthcareFacilities.Include(p => p.Uploads).FirstOrDefault(p => p.UserId == request.UserId);
            if (healthCareFacility == null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }
            healthCareFacility.ChangeUploadVisibilityToPublic(request.UploadId);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Updated;
        }

    }
}