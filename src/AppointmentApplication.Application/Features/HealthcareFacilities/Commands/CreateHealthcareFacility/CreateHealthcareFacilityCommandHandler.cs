using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

public class CreateHealthcareFacilityCommandHandler(
    ILogger<CreateHealthcareFacilityCommandHandler> logger,
    IAppDbContext context,
    IAuthenticationService authenticationService)
    : IRequestHandler<CreateHealthcareFacilityCommand, Result<HealthCareFacility>>
{
    private readonly ILogger<CreateHealthcareFacilityCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result<HealthCareFacility>> Handle(
        CreateHealthcareFacilityCommand request,
        CancellationToken cancellationToken)
    {
            // 1. Create User
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == "HealthCareFacility", cancellationToken)
                ?? Role.HealthCareFacility;

            var user = User.Create(Guid.NewGuid(), request.FirstName, request.LastName, request.Email, role);

            // Check if user with email already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (existingUser != null)
            {
                return ApplicationHealthCareFacilityErrors.UserAlreadyExists(request.Email);
            }

            // Register user in authentication system
            string identityId = await _authenticationService.RegisterAsync(user, request.Password, cancellationToken);
            user.SetIdentityId(identityId);

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
                user.Id, // Use the newly created user's ID
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

            // 6. Save both user and facility to database
            _context.Users.Add(user);
            _context.HealthcareFacilities.Add(createHealthCareFacilityResult.Value);

            var saveResult = await _context.SaveChangesAsync(cancellationToken);

            if (saveResult <= 0)
            {
                _logger.LogError("Failed to save healthcare facility and user to database.");
                return ApplicationHealthCareFacilityErrors.DatabaseSaveFailed("No changes were saved to the database");
            }

            // 7. Log success and return result
            var healthCareFacility = createHealthCareFacilityResult.Value;
            _logger.LogInformation(
                "Healthcare Facility and User Created Successfully. Facility ID: {HealthCareFacilityId}, User ID: {UserId}, Name: {FacilityName}",
                healthCareFacility.Id, user.Id, healthCareFacility.Name);

            return healthCareFacility;
    }

    private static bool IsValidCoordinates(double latitude, double longitude)
    {
        return latitude >= -90 && latitude <= 90 &&
               longitude >= -180 && longitude <= 180;
    }
}