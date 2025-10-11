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

namespace AppointmentApplication.Application.Features.Patients.Allergies.Queries.GetAllergies
{
    public class GetAllergiesQueryHandler : IRequestHandler<GetAllergiesQuery, Result<List<Allergy>>>
    {
        private readonly IAppDbContext _context;
        public GetAllergiesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<Allergy>>> Handle(GetAllergiesQuery request, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients
                .Include(p => p.Allergies)
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);
            if (patient is null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }
            return patient.Allergies.ToList();
        }
    }
}