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
    public class AddAllergyCommandHandler : IRequestHandler<AddAllergyCommand, Result<Allergy>>
    {
        private readonly IAppDbContext _context;

        // ✅ Constructor Injection
        public AddAllergyCommandHandler(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result<Allergy>> Handle(AddAllergyCommand request, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients
                .Include(p => p.Allergies)
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

            if (patient is null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }

            var allergy = await _context.Allergies
                    .FirstOrDefaultAsync(a => a.Name == request.AllergyType, cancellationToken);

            if (allergy is null)
            {
                return ApplicationPatientErrors.InvalidAllergyType;
            }
            var allergyResult = patient.AddAllergy(allergy);

            if (allergyResult.IsError)
            {
                return allergyResult.Errors;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return allergyResult.Value; // الآن Value ليس null
        }

    }
}