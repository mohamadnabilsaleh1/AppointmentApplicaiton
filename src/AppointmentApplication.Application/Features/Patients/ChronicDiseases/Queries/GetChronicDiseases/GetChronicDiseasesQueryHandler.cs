using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Errors;

using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Patients.ChronicDiseases;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Patients.ChronicDiseases.Queries.GetChronicDiseases
{
    public class GetChronicDiseasesQueryHandler : IRequestHandler<GetChronicDiseasesQuery, Result<List<ChronicDisease>>>
    {
        private readonly IAppDbContext _context;
        public GetChronicDiseasesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<ChronicDisease>>> Handle(GetChronicDiseasesQuery request, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients
                .Include(p => p.ChronicDiseases)
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);
            if (patient is null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }
            return patient.ChronicDiseases.ToList();
        }

    }
}