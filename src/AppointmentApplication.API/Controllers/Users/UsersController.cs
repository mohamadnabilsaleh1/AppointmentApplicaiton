using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Abstractions.Authentication;

using AppointmentApplication.Application.Features.Users.AddAvatar;

using AppointmentApplication.Application.Features.Users.Dtos;
using AppointmentApplication.Application.Features.Users.GetAvatar;

using AppointmentApplication.Application.Features.Users.GetLoggedInUser;

using AppointmentApplication.Application.Features.Users.LogInUser;
using AppointmentApplication.Application.Features.Users.RegisterPatient;

using AppointmentApplication.Application.Features.Users.RegisterUser;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Contracts.Requests;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApplication.API.Controllers;

[Route("/api/users")]
public class UsersController(ISender sender, IEmailSender emailSender, IUserContext userContext) : ApiController
{
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUserContext _userContext = userContext;

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
    // اختبر في controller منفصل
    [HttpPost("test-email")]
    public async Task<IActionResult> TestEmail()
    {
        try
        {
            var to = "gamer2mohamad@gmail.com";
            var subject = "Test Email - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var body = @"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Test Email</title>
            </head>
            <body>
                <h1>Test Email from Motorex Expo</h1>
                <p>This is a test email sent from our ASP.NET Core application.</p>
                <p><strong>Time:</strong> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"</p>
                <p><strong>From:</strong> info@motorexexpo.com</p>
                <p><strong>To:</strong> " + to + @"</p>
                <hr>
                <p>If you can see this email, the system is working correctly!</p>
            </body>
            </html>";

            await _emailSender.SendEmailAsync(to, subject, body);

            return Ok($"Email sent successfully to {to} at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        }
        catch (Exception ex)
        {
            // _logger.LogError(ex, "Failed to send test email");
            return StatusCode(500, new
            {
                message = "Failed to send email",
                error = ex.Message,
                details = ex.InnerException?.Message
            });
        }
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
    /// <summary>
    /// Uploads or updates current user avatar.
    /// </summary>
    /// <param name="file">The avatar image file</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Success or error response</returns>
    [HttpPut("me/avatar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Uploads or updates current user avatar.")]
    [EndpointDescription("Uploads an avatar image for the currently authenticated user.")]
    [EndpointName("AddMyAvatar")]
    public async Task<IActionResult> AddMyAvatar(
        AddAvatarRequest request,
        CancellationToken cancellationToken)
    {

        var result = await sender.Send(new AddAvatarCommand(_userContext.UserId, request.File), cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
    /// <summary>
    /// Gets user avatar.
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Avatar file or error response</returns>

    /// <summary>
    /// Gets current user avatar.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Avatar file or error response</returns>
    // [HttpGet("me/avatar")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // [EndpointSummary("Gets current user avatar.")]
    // [EndpointDescription("Retrieves the avatar image for the currently authenticated user.")]
    // [EndpointName("GetMyAvatar")]
    // public async Task<IActionResult> GetMyAvatar(CancellationToken cancellationToken)
    // {
    //     // Implement based on your auth system

    //     var result = await sender.Send(new GetAvatarQuery(_userContext.UserId), cancellationToken);

    //     return result.Match<IActionResult>(
    //         fileResponse =>
    //         {
    //             return File(fileResponse.FileBytes, fileResponse.ContentType);
    //         },
    //         Problem);
    // }
    [HttpGet("{id:guid}/avatar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Gets current user avatar.")]
    [EndpointDescription("Retrieves the avatar image for the currently authenticated user.")]
    [EndpointName("GetMyAvatarPublic")]
    public async Task<IActionResult> GetMyAvatarPublic(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAvatarQuery(id), cancellationToken);

        return result.Match(
            fileResponse =>
            {
                var fileResult = File(fileResponse.FileBytes, fileResponse.ContentType);
                fileResult.FileDownloadName = null; // Remove download name to display inline

                // Alternative: Set content disposition header directly
                Response.Headers.Append("Content-Disposition", $"inline; filename=\"{fileResponse.FileName ?? "avatar"}\"");

                return fileResult;
            },
            Problem);
    }

}
