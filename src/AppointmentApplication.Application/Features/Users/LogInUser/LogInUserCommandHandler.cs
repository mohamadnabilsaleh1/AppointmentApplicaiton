using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Users.Errors;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Users.LogInUser;

public sealed class LogInUserCommandHandler(
    ILogger<LogInUserCommandHandler> logger,
    IJwtService jwtService)
    : IRequestHandler<LogInUserCommand, Result<AccessTokenResponse>>
{
    private readonly ILogger<LogInUserCommandHandler> _logger = logger;
    private readonly IJwtService _jwtService = jwtService;

    public async Task<Result<AccessTokenResponse>> Handle(
        LogInUserCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt started for email {Email}", request.Email);

        var tokenResult = await _jwtService.GetAccessTokenAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (tokenResult.IsError)
        {
            _logger.LogWarning("Login failed for email {Email}. Invalid credentials.", request.Email);
            return ApplicationUserErrors.InvalidCredentials;
        }

        var response = new AccessTokenResponse(tokenResult.Value);

        _logger.LogInformation("Login successful for email {Email}.", request.Email);

        return response;
    }
}
