using System;
using System.Threading;
using System.Threading.Tasks;
using AppointmentApplication.Application.Features.Doctors.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Doctors.Commands.AddDescription
{
    public class AddDescriptionCommandHandler : IRequestHandler<AddDescriptionCommand, Result<Updated>>
    {
        private readonly ILogger<AddDescriptionCommandHandler> _logger;
        private readonly IAppDbContext _context;

        public AddDescriptionCommandHandler(
            ILogger<AddDescriptionCommandHandler> logger,
            IAppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result<Updated>> Handle(
            AddDescriptionCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Find the existing doctor
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);

            if (doctor is null)
            {
                _logger.LogWarning("Doctor not found. ID: {DoctorId}", request.UserId);
                return ApplicationDoctorErrors.DoctorNotFound(request.UserId);
            }

            // 2. Validate description (optional - add your validation rules)
            if (string.IsNullOrWhiteSpace(request.Description))
            {
                _logger.LogWarning("Description cannot be empty for doctor ID: {DoctorId}", request.UserId);
                return DoctorErrors.InvalidDescription;
            }

            // 3. Update the description using the domain method
            var updateResult = doctor.SetDescription(request.Description);

            if (updateResult.IsError)
            {
                _logger.LogWarning(
                    "Doctor description update failed: {Errors}",
                    string.Join(", ", updateResult.Errors));
                return updateResult.Errors;
            }

            // 4. Save changes
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Description updated successfully for doctor ID: {DoctorId}", request.UserId);
            return Result.Updated;
        }
    }
}