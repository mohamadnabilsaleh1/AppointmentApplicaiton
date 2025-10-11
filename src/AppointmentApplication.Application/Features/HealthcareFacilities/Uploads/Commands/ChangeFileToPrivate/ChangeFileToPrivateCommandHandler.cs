using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Features.Patients.Errors;

using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Commands.ChangeFileToPrivate
{
    public class ChangeFileToPrivateCommandHandler : IRequestHandler<ChangeFileToPrivateCommand, Result<Updated>>
    {
        private readonly IAppDbContext _context;
        public ChangeFileToPrivateCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<Updated>> Handle(ChangeFileToPrivateCommand request, CancellationToken cancellationToken)
        {
            var healthCareFacility = _context.HealthcareFacilities.Include(h => h.Uploads).FirstOrDefault(p => p.UserId == request.UserId);
            if (healthCareFacility == null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
            }
            healthCareFacility.ChangeUploadVisibilityToPrivate(request.UploadId);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Updated;
        }

    }
}