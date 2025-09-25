using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.UpdateHealthcareFacility;

public class UpdateHealthcareFacilityCommandHandler(
    ILogger<UpdateHealthcareFacilityCommandHandler> logger,
    IAppDbContext context)
    : IRequestHandler<UpdateHealthcareFacilityCommand, Result<HealthCareFacility>>
{
    private readonly ILogger<UpdateHealthcareFacilityCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;

    public async Task<Result<HealthCareFacility>> Handle(
        UpdateHealthcareFacilityCommand request,
        CancellationToken cancellationToken)
    {
        try
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
                request.Type,
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
            var saveResult = await _context.SaveChangesAsync(cancellationToken);

            if (saveResult <= 0)
            {
                _logger.LogError("Failed to save healthcare facility update to database. Facility ID: {FacilityId}", request.FacilityId);
                return ApplicationHealthCareFacilityErrors.DatabaseSaveFailed("No changes were saved to the database");
            }

            // 8. Reload the facility to get updated data
            var updatedFacility = await _context.HealthcareFacilities
                .FirstOrDefaultAsync(f => f.Id == request.FacilityId, cancellationToken);

            _logger.LogInformation(
                "Healthcare Facility Updated Successfully. ID: {FacilityId}, Name: {FacilityName}",
                request.FacilityId, request.Name);

            return updatedFacility!;
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database error occurred while updating healthcare facility. ID: {FacilityId}", request.FacilityId);
            return ApplicationHealthCareFacilityErrors.DatabaseSaveFailed(dbEx.InnerException?.Message ?? dbEx.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while updating healthcare facility. ID: {FacilityId}", request.FacilityId);
            return Error.Failure("HealthCareFacility.Update.UnexpectedError", $"An unexpected error occurred: {ex.Message}");
        }
    }

    private static bool IsValidCoordinates(double latitude, double longitude)
    {
        return latitude >= -90 && latitude <= 90 &&
               longitude >= -180 && longitude <= 180;
    }
}