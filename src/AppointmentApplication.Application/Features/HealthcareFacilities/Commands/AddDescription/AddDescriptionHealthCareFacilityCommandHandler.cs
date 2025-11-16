using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Commands.AddDescription;

using AppointmentApplication.Application.Features.Doctors.Errors;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.AddDescription
{
    public class AddDescriptionHealthCareFacilityCommandHandler : IRequestHandler<AddDescriptionHealthCareFacilityCommand, Result<Updated>>
    {
        private readonly ILogger<AddDescriptionHealthCareFacilityCommandHandler> _logger;
        private readonly IAppDbContext _context;

        public AddDescriptionHealthCareFacilityCommandHandler(
            ILogger<AddDescriptionHealthCareFacilityCommandHandler> logger,
            IAppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result<Updated>> Handle(
            AddDescriptionHealthCareFacilityCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Find the existing doctor

            Console.WriteLine("descripton =======++>"+request.Description + "==============<");
            var healthCareFacility = await _context.HealthcareFacilities
                .FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);

            if (healthCareFacility is null)
            {
                _logger.LogWarning("HealthCareFacility not found. ID: {HealthCareFacilityId}", request.UserId);
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
            }

            // 2. Validate description (optional - add your validation rules)
            if (string.IsNullOrWhiteSpace(request.Description))
            {
                _logger.LogWarning("Description cannot be empty for HealthCareFacility ID: {HealthCareFacilityId}", request.UserId);
                return ApplicationHealthCareFacilityErrors.InvalidDescription;
            }

            // 3. Update the description using the domain method
            var updateResult = healthCareFacility.SetDescription(request.Description);

            if (updateResult.IsError)
            {
                _logger.LogWarning(
                    "HealthCareFacility description update failed: {Errors}",
                    string.Join(", ", updateResult.Errors));
                return updateResult.Errors;
            }

            // 4. Save changes
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Description updated successfully for HealthCareFacility ID: {HealthCareFacilityId}", request.UserId);
            return Result.Updated;
        }
    }
}