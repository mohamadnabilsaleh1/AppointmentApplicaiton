using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Users.RegisterUser;
using AppointmentApplication.Contracts.Requests;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApplication.API.Controllers;

[Route("/api/users")]
[ApiController]
public class UsersController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">The user registration details.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created user ID or error details.</returns>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Registers a new user.")]
    [EndpointDescription("Creates a new user account in the system.")]
    [EndpointName("RegisterUser")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);

        Result<Guid> result = await sender.Send(command, cancellationToken);

        return result.Match(
            userId =>
                CreatedAtAction(
                    actionName: nameof(GetUserById),
                    routeValues: new { id = userId },
                    value: new { Id = userId, request.Email, request.FirstName, request.LastName }),
            errors => Problem(
                detail: string.Join(", ", errors.Select(e => e.ToString())),
                statusCode: StatusCodes.Status400BadRequest
            )
        );
    }

    /// <summary>
    /// Gets a user by ID (stub for CreatedAtAction reference).
    /// </summary>
    [HttpGet("{id:guid}", Name = nameof(GetUserById))]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetUserById(Guid id)
    {
        // Just a placeholder endpoint for CreatedAtAction.
        return Ok(new { Id = id });
    }
}
