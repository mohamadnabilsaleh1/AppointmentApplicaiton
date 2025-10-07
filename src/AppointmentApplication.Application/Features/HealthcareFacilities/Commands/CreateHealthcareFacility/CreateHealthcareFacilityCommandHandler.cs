using AppointmentApplication.Application.Abstractions.Authentication;
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

        var createUser = User.Create(Guid.NewGuid(), request.FirstName, request.LastName, request.Email, role);
        if (createUser.IsError)
        {
            _logger.LogWarning(
                "User creation failed: {Errors}",
                string.Join(", ", createUser.Errors));
            return createUser.Errors;
        }

        var user = createUser.Value;

        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (existingUser != null)
        {
            return ApplicationHealthCareFacilityErrors.UserAlreadyExists(request.Email);
        }

        string identityId = await _authenticationService.RegisterAsync(user, request.Password, cancellationToken);
        user.SetIdentityId(identityId);

        // 2. Check facility name uniqueness
        var nameExists = await _context.HealthcareFacilities
            .AnyAsync(f => f.Name == request.Name, cancellationToken);

        if (nameExists)
        {
            _logger.LogWarning("Healthcare facility creation aborted. Facility name '{FacilityName}' already exists.", request.Name);
            return ApplicationHealthCareFacilityErrors.FacilityNameAlreadyExists(request.Name);
        }

        // 3. Create address
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

        // 4. Create healthcare facility (domain enforces coordinates & invariants)
        Result<HealthCareFacility> createHealthCareFacilityResult = HealthCareFacility.Create(
            Guid.NewGuid(),
            user.Id,
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

        // 5. Save both user + facility
        _context.Users.Add(user);
        _context.HealthcareFacilities.Add(createHealthCareFacilityResult.Value);

        await _context.SaveChangesAsync(cancellationToken);

        var healthCareFacility = createHealthCareFacilityResult.Value;
        _logger.LogInformation(
            "Healthcare Facility and User Created Successfully. Facility ID: {HealthCareFacilityId}, User ID: {UserId}, Name: {FacilityName}",
            healthCareFacility.Id, user.Id, healthCareFacility.Name);

        return healthCareFacility;
    }
}
