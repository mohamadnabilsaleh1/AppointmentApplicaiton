using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Errors;

using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Patients.Uploads.Commands.ChangeFileToPrivate
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
            var patient = _context.Patients.Include(p=> p.Uploads).FirstOrDefault(p => p.UserId == request.UserId);
            if (patient == null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }
            patient.ChangeUploadVisibilityToPrivate(request.UploadId);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Updated;
        }

    }
}