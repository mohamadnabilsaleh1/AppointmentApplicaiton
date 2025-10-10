using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Errors;

using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Patients;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Patients.Allergies.Commands.DeleteAllergy
{
    public class DeleteAllergyCommandHandler : IRequestHandler<DeleteAllergyCommand, Result<Deleted>>
    {
        private readonly IAppDbContext _context;

        // ✅ Constructor Injection
        public DeleteAllergyCommandHandler(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result<Deleted>> Handle(DeleteAllergyCommand request, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients
                .Include(p => p.Allergies)
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);
            if (patient == null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }

            patient.DeleteAllergy(request.AllergyId);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Deleted;
        }

    }
}