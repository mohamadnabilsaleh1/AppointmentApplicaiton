using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.DeactivateHealthcareFacilityById;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands;

public class DeactivateHealthcareFacilityByIdCommandHandler
    : IRequestHandler<DeactivateHealthcareFacilityByIdCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<DeactivateHealthcareFacilityByIdCommandHandler> _logger;

    public DeactivateHealthcareFacilityByIdCommandHandler(
        IAppDbContext context,
        ILogger<DeactivateHealthcareFacilityByIdCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Updated>> Handle(DeactivateHealthcareFacilityByIdCommand request, CancellationToken cancellationToken)
    {
        var facility = await _context.HealthcareFacilities
            .FirstOrDefaultAsync(f => f.Id == request.FacilityId, cancellationToken);

        if (facility == null)
        {
            _logger.LogWarning("Facility not found for deactivation. ID: {FacilityId}", request.FacilityId);
            return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.FacilityId);
        }

        facility.Deactivate(); // ✅ Domain method

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}
