using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Dtos.Reviews;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Reviews.Commands.CreateReview;
using AppointmentApplication.Application.Features.Reviews.Dtos;
using AppointmentApplication.Application.Features.Reviews.Queries.GetReviewByAppointmentId;
using AppointmentApplication.Application.Features.Reviews.Queries.GetReviewsByHealthCareFacilityId;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Contracts.Requests.Reviews;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/patients/me/appointments/{appointmentId:guid}/reviews")]
[Authorize(Roles = Roles.Patient)]
public sealed class PatientReviewController : ApiController
{
    private readonly ISender _sender;
    private readonly LinkService _linkService;
    private readonly IUserContext _userContext;

    public PatientReviewController(ISender sender, LinkService linkService, IUserContext userContext)
    {
        _sender = sender;
        _linkService = linkService;
        _userContext = userContext;
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Patient}")]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Creates a new Review.")]
    [EndpointDescription("Adds a new Review for a completed appointment.")]
    [EndpointName("CreateReview")]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request, Guid appointmentId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new CreateReviewCommand(
                _userContext.UserId,
                appointmentId,
                request.Rating,
                request.Comment),
            cancellationToken);

        return result.Match(
            response =>
            {
                var links = CreateLinksForReview(response.Id.ToString(), response.AppointmentId.ToString());

                var resource = new
                {
                    data = response,
                    links
                };

                // Using CreatedAtAction instead of CreatedAtRoute
                return CreatedAtAction(
                    actionName: nameof(GetReviewByAppointmentId),
                    controllerName: "PatientReview", // Controller name without "Controller" suffix
                    routeValues: new { appointmentId = response.AppointmentId, apiVersion = "0.1" },
                    value: resource);
            },
            Problem);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Review by Appointment Id.")]
    [EndpointDescription("Retrieves a review for a specific appointment.")]
    [EndpointName("GetReviewByAppointmentId")]
    public async Task<IActionResult> GetReviewByAppointmentId(Guid appointmentId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetReviewByAppointmentIdQuery(appointmentId), cancellationToken);

        return result.Match(
            review =>
            {
                // var links = CreateLinksForReview(review.Id.ToString(), review.AppointmentId.ToString());

                var resource = new
                {
                    data = review
                };
                return Ok(resource);
            },
            Problem);
    }

    private List<LinkDto> CreateLinksForReview(string reviewId, string appointmentId)
    {
        var links = new List<LinkDto>
        {
            _linkService.Create(nameof(GetReviewByAppointmentId), "self", HttpMethods.Get, new { appointmentId }),
            _linkService.Create(nameof(CreateReview), "create", HttpMethods.Post)
        };

        return links;
    }
}