using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Dtos;
using AppointmentApplication.Application.Features.Patients.Errors;
using AppointmentApplication.Application.Features.Patients.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Patients;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Patients.Queries.GetPatientByUserId
{
    public class GetPatientByUserIdQueryHandler : IRequestHandler<GetPatientByUserIdQuery, Result<PatientDto>>
    {
        private readonly IAppDbContext _context;

        public GetPatientByUserIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PatientDto>> Handle(GetPatientByUserIdQuery request, CancellationToken cancellationToken)
        {
            var patients = await _context.Patients
                .ToListAsync(cancellationToken);
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);
            if (patient is null)
            {
                return PatientErrors.PatientNotFound;
            }
            return patient.ToDto();
        }
    }
}
