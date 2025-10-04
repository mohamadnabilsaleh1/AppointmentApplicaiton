using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.ActivateHealthcareFacilityById;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands;

public class ActivateHealthcareFacilityByIdCommandHandler
    : IRequestHandler<ActivateHealthcareFacilityByIdCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<ActivateHealthcareFacilityByIdCommandHandler> _logger;

    public ActivateHealthcareFacilityByIdCommandHandler(
        IAppDbContext context,
        ILogger<ActivateHealthcareFacilityByIdCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Updated>> Handle(ActivateHealthcareFacilityByIdCommand request, CancellationToken cancellationToken)
    {
        var facility = await _context.HealthcareFacilities
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(f => f.Id == request.FacilityId, cancellationToken);

        if (facility == null)
        {
            _logger.LogWarning("Facility not found for activation. ID: {FacilityId}", request.FacilityId);
            return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.FacilityId);
        }

        facility.Activate(); // ✅ Domain method

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}