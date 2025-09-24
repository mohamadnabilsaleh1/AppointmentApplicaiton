using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

public class CreateHealthcareFacilityCommandHandler(ILogger<CreateHealthcareFacilityCommand> logger,
    IAppDbContext context) : IRequestHandler<CreateHealthcareFacilityCommand, Result<HealthCareFacility>>
{
    private readonly ILogger<CreateHealthcareFacilityCommand> _logger = logger;
    private readonly IAppDbContext _context = context;
    public async Task<Result<HealthCareFacility>> Handle(CreateHealthcareFacilityCommand request, CancellationToken cancellationToken)
    {
        bool exist = await _context.HealthcareFacilities.AnyAsync(f => f.UserId == request.UserId, cancellationToken);
        if (exist)
        {
            _logger.LogWarning("HealthCareFacility creation aborted. UserId already exists.");
        }
        Result<Address> createAddressResult = Address.Create(request.Street, request.City, request.State, request.Country, request.ZipCode);
        if (createAddressResult.IsError)
        {
            return createAddressResult.Errors;
        }

        Result<HealthCareFacility> createHealthCareFacilityResult = HealthCareFacility.Create(request.UserId, request.Name, request.Type, createAddressResult.Value, request.GPSLatitude, request.GPSLongitude);

        if (createHealthCareFacilityResult.IsError)
        {
            return createHealthCareFacilityResult.Errors;
        }
        _context.HealthcareFacilities.Add(createHealthCareFacilityResult.Value);
        await _context.SaveChangesAsync(cancellationToken);

        HealthCareFacility healthCareFacility = createHealthCareFacilityResult.Value;
        _logger.LogInformation("Healthcare Facility Created Successfully. id:{HealthCareFacilityId} ", healthCareFacility.Id);
        return healthCareFacility;
    }
}

