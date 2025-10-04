using System;
using System.Threading;
using System.Threading.Tasks;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.UpdateHealthcareFacility;

public class UpdateHealthcareFacilityByIdCommandHandler 
    : IRequestHandler<UpdateHealthcareFacilityByIdCommand, Result<Updated>>
{
    private readonly ILogger<UpdateHealthcareFacilityByIdCommandHandler> _logger;
    private readonly IAppDbContext _context;

    public UpdateHealthcareFacilityByIdCommandHandler(
        ILogger<UpdateHealthcareFacilityByIdCommandHandler> logger,
        IAppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Updated>> Handle(
        UpdateHealthcareFacilityByIdCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Find the existing facility
        var facility = await _context.HealthcareFacilities
            .FirstOrDefaultAsync(f => f.Id == request.FacilityId, cancellationToken);

        if (facility == null)
        {
            _logger.LogWarning("Healthcare facility not found. ID: {FacilityId}", request.FacilityId);
            return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.FacilityId);
        }

        // 2. Check if facility is active
        if (!facility.IsActive)
        {
            _logger.LogWarning("Cannot update inactive healthcare facility. ID: {FacilityId}", request.FacilityId);
            return ApplicationHealthCareFacilityErrors.FacilityInactive(request.FacilityId);
        }

        // 3. Check if name is being changed and validate uniqueness
        if (!string.Equals(facility.Name, request.Name, StringComparison.OrdinalIgnoreCase))
        {
            var nameExists = await _context.HealthcareFacilities
                .AnyAsync(f => f.Name == request.Name && f.Id != request.FacilityId, cancellationToken);

            if (nameExists)
            {
                _logger.LogWarning("Healthcare facility name already exists. Name: {FacilityName}", request.Name);
                return ApplicationHealthCareFacilityErrors.FacilityNameAlreadyExists(request.Name);
            }
        }

        // 4. Validate GPS coordinates
        if (!IsValidCoordinates(request.GPSLatitude, request.GPSLongitude))
        {
            _logger.LogWarning("Invalid GPS coordinates provided: Lat {Latitude}, Long {Longitude}",
                request.GPSLatitude, request.GPSLongitude);
            return ApplicationHealthCareFacilityErrors.InvalidCoordinates;
        }

        // 5. Create new address
        Result<Address> createAddressResult = Address.Create(
            request.Street,
            request.City,
            request.State,
            request.Country,
            request.ZipCode);

        if (createAddressResult.IsError)
        {
            _logger.LogWarning("Address creation failed: {Errors}",
                string.Join(", ", createAddressResult.Errors));
            return createAddressResult.Errors;
        }

        // 6. Update the facility
        var updateResult = facility.Update(
            request.Name,
            createAddressResult.Value,
            request.GPSLatitude,
            request.GPSLongitude);

        if (updateResult.IsError)
        {
            _logger.LogWarning("Healthcare facility update failed: {Errors}",
                string.Join(", ", updateResult.Errors));
            return updateResult.Errors;
        }

        // 7. Save changes
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }

    private static bool IsValidCoordinates(double latitude, double longitude)
    {
        return latitude >= -90 && latitude <= 90 &&
               longitude >= -180 && longitude <= 180;
    }
}
