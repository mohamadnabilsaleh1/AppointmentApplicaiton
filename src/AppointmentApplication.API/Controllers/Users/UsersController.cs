using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Users.Dtos;

using AppointmentApplication.Application.Features.Users.GetLoggedInUser;

using AppointmentApplication.Application.Features.Users.LogInUser;
using AppointmentApplication.Application.Features.Users.RegisterPatient;

using AppointmentApplication.Application.Features.Users.RegisterUser;
using AppointmentApplication.Contracts.Requests;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApplication.API.Controllers;

[Route("/api/users")]
public class UsersController(ISender sender) : ApiController
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
            Problem);
    }

    /// <summary>
    /// Registers a new patient.
    /// </summary>
    /// <param name="request">The patient registration details.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created patient ID or error details.</returns>
    [AllowAnonymous]
    [HttpPost("register-patient")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Registers a new patient.")]
    [EndpointDescription("Creates a new patient account in the system.")]
    [EndpointName("RegisterPatient")]
    public async Task<IActionResult> RegisterPatient(
        [FromBody] RegisterPatientRequest request,
        CancellationToken cancellationToken)
    {
        /*string PhoneNumber, long NationalId, string Email, string Password*/
        var command = new RegisterPatientCommand(
            request.PhoneNumber,
            request.NationalId,
            request.Email,
            request.Password);

        Result<Guid> result = await sender.Send(command, cancellationToken);

        return result.Match(
            patientId =>
                CreatedAtAction(
                    actionName: nameof(GetUserById),
                    routeValues: new { id = patientId },
                    value: new { Id = patientId, request.Email, request.PhoneNumber, request.NationalId }),
            Problem);
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

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Logs in a user.")]
    [EndpointDescription("Authenticates a user and returns an access token.")]
    [EndpointName("LogInUser")]
    public async Task<IActionResult> LogIn(
        [FromBody] LogInUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LogInUserCommand(request.Email, request.Password);

        Result<AccessTokenResponse> result = await sender.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            accessToken => Ok(accessToken),
            Problem);
    }

    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Gets the currently logged-in user.")]
    [EndpointDescription("Returns details of the currently authenticated user.")]
    [EndpointName("GetLoggedInUser")]
    public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken)
    {
        // إرسال استعلام للحصول على بيانات المستخدم الحالي
        var query = new GetLoggedInUserQuery();

        Result<UserDto> result = await sender.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            user => Ok(user),    // إذا تم العثور على المستخدم نرجع 200 OK مع بياناته
            Problem);              // إذا لم يتم العثور أو حدث خطأ نرجع ProblemDetails
    }
}
