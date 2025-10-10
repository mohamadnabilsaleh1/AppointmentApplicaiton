using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Errors;

using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Patients.Allergies;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Patients.Commands.AddAllergy
{
    public class AddAllergyCommandHandler : IRequestHandler<AddAllergyCommand, Result<Created>>
    {
        private readonly IAppDbContext _context;

        // ✅ Constructor Injection
        public AddAllergyCommandHandler( IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result<Created>> Handle(AddAllergyCommand request, CancellationToken cancellationToken)
        {
            var allergy = await _context.Allergies
                .FirstOrDefaultAsync(a => a.Name == request.AllergyType, cancellationToken) ?? Allergy.GetAll().FirstOrDefault(a => a.Name == request.AllergyType);
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

            if (patient is null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }

            patient.AddAllergy(allergy!);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Created;
        }

    }
}