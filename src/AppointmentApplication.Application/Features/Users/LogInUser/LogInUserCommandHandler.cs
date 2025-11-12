// Application/Features/Users/LogInUser/LogInUserCommandHandler.cs
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Users.Errors;
using AppointmentApplication.Application.Features.Users.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Users.LogInUser;

public sealed class LogInUserCommandHandler : IRequestHandler<LogInUserCommand, Result<AccessTokenResponse>>
{
    private readonly ILogger<LogInUserCommandHandler> _logger;
    private readonly IJwtService _jwtService;
    private readonly IAppDbContext _context;

    public LogInUserCommandHandler(
        ILogger<LogInUserCommandHandler> logger,
        IJwtService jwtService,
        IAppDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Result<AccessTokenResponse>> Handle(
        LogInUserCommand request,
        CancellationToken cancellationToken)
    {

            _logger.LogInformation("Login attempt started for email {Email}", request.Email);

            // Validate user exists first
            var user = await _context.Users
            .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user is null)
            {
                _logger.LogWarning("Login failed for email {Email}. User not found.", request.Email);
                return ApplicationUserErrors.InvalidCredentials;
            }

            // Get token with credentials
            var tokenResult = await _jwtService.GetAccessTokenAsync(
                request.Email,
                request.Password,
                cancellationToken);

            if (tokenResult.IsError)
            {
                _logger.LogWarning("Login failed for email {Email}. Invalid credentials.", request.Email);
                return ApplicationUserErrors.InvalidCredentials;
            }

            // Map user to DTO
            var userDto = user.ToDto();

            // Create response with token and user data
            var response = new AccessTokenResponse(tokenResult.Value, userDto);

            _logger.LogInformation(
                "Login successful for email {Email}. User: {UserId}, Role: {UserRole}",
                request.Email, user.Id, user.Roles);

            return response;

    }
}