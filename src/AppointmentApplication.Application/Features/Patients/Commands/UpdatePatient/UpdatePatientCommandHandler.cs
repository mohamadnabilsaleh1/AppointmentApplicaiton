using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Errors;

using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Patients.Commands.UpdatePatient
{
    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Result<Updated>>
    {
        private readonly ILogger<UpdatePatientCommandHandler> _logger;
        private readonly IAppDbContext _context;

        public UpdatePatientCommandHandler(
            ILogger<UpdatePatientCommandHandler> logger,
            IAppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result<Updated>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);
            if (patient is null)
            {
                _logger.LogWarning("Patient not found. ID: {PatientId}", request.UserId);
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }

            var updateResult = patient.Update(request.NationalId, request.Gender, request.DateOfBirth);

            if (updateResult.IsError)
            {
                _logger.LogWarning(
                    "Patient update failed: {Errors}",
                    string.Join(", ", updateResult.Errors));
                return updateResult.Errors;
            }

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Patient updated successfully. ID: {PatientId}", request.UserId);
            return Result.Updated;
        }
    }
}