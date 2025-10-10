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

namespace AppointmentApplication.Application.Features.Patients.ChronicDiseases.Commands.DeleteChronicDisease
{
    public class DeleteChronicDiseaseCommandHandler : IRequestHandler<DeleteChronicDiseaseCommand, Result<Deleted>>
    {
        private readonly IAppDbContext _context;

        // ✅ Constructor Injection
        public DeleteChronicDiseaseCommandHandler(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result<Deleted>> Handle(DeleteChronicDiseaseCommand request, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients
    .Include(p => p.ChronicDiseases)
    .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);
            if (patient == null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }

            var chronicDisease = await _context.ChronicDiseases
                .FirstOrDefaultAsync(a => a.Name == request.ChronicDiseaseType, cancellationToken);
            if (chronicDisease == null)
            {
                return ApplicationPatientErrors.AllergyNotFound(request.UserId);
            }

            patient.DeleteChronicDisease(chronicDisease);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }

    }
}