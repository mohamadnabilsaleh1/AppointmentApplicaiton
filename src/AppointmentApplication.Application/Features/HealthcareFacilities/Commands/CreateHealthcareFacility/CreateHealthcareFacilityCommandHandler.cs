using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

public class CreateHealthcareFacilityCommandHandler(
    ILogger<CreateHealthcareFacilityCommandHandler> logger, // Fixed logger type
    IAppDbContext context)
    : IRequestHandler<CreateHealthcareFacilityCommand, Result<HealthCareFacility>>
{
    private readonly ILogger<CreateHealthcareFacilityCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;

    public async Task<Result<HealthCareFacility>> Handle(
        CreateHealthcareFacilityCommand request,
        CancellationToken cancellationToken)
    {

        var createUser = User.Create(Guid.NewGuid(), request.FirstName, request.LastName, request.Email);
        var userId = "wslfsflfkajsdf";

        // 1. Validate user doesn't already have a facility
        var userFacilityExists = await _context.HealthcareFacilities
            .AnyAsync(f => f.UserId == userId, cancellationToken);

        if (userFacilityExists)
        {
            _logger.LogWarning("Healthcare facility creation aborted. User {UserId} already has a facility.", userId);
            return ApplicationHealthCareFacilityErrors.FacilityAlreadyExists(userId);
        }

        // 2. Validate facility name is unique
        var nameExists = await _context.HealthcareFacilities
            .AnyAsync(f => f.Name == request.Name, cancellationToken);

        if (nameExists)
        {
            _logger.LogWarning("Healthcare facility creation aborted. Facility name '{FacilityName}' already exists.", request.Name);
            return ApplicationHealthCareFacilityErrors.FacilityNameAlreadyExists(request.Name);
        }

        // 3. Validate GPS coordinates
        if (!IsValidCoordinates(request.GPSLatitude, request.GPSLongitude))
        {
            _logger.LogWarning(
                "Invalid GPS coordinates provided: Lat {Latitude}, Long {Longitude}",
                request.GPSLatitude, request.GPSLongitude);
            return ApplicationHealthCareFacilityErrors.InvalidCoordinates;
        }

        // 4. Create address
        Result<Address> createAddressResult = Address.Create(
            request.Street,
            request.City,
            request.State,
            request.Country,
            request.ZipCode);

        if (createAddressResult.IsError)
        {
            _logger.LogWarning(
                "Address creation failed: {Errors}",
                string.Join(", ", createAddressResult.Errors));
            return createAddressResult.Errors;
        }

        // 5. Create healthcare facility
        Result<HealthCareFacility> createHealthCareFacilityResult = HealthCareFacility.Create(
            Guid.NewGuid(),
            userId,
            request.Name,
            request.Type,
            createAddressResult.Value,
            request.GPSLatitude,
            request.GPSLongitude);

        if (createHealthCareFacilityResult.IsError)
        {
            _logger.LogWarning(
                "Healthcare facility creation failed: {Errors}",
                string.Join(", ", createHealthCareFacilityResult.Errors));
            return createHealthCareFacilityResult.Errors;
        }

        // 6. Save to database
        _context.HealthcareFacilities.Add(createHealthCareFacilityResult.Value);

        var saveResult = await _context.SaveChangesAsync(cancellationToken);

        if (saveResult <= 0)
        {
            _logger.LogError("Failed to save healthcare facility to database. Save result: {SaveResult}", saveResult);
            return ApplicationHealthCareFacilityErrors.DatabaseSaveFailed("No changes were saved to the database");
        }

        // 7. Log success and return result
        var healthCareFacility = createHealthCareFacilityResult.Value;
        _logger.LogInformation(
            "Healthcare Facility Created Successfully. ID: {HealthCareFacilityId}, Name: {FacilityName}",
            healthCareFacility.Id, healthCareFacility.Name);

        return healthCareFacility;

    }

    private static bool IsValidCoordinates(double latitude, double longitude)
    {
        return latitude >= -90 && latitude <= 90 &&
               longitude >= -180 && longitude <= 180;
    }
}