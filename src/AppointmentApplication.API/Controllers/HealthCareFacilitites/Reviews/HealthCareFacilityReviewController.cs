using System.Dynamic;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Dtos.Reviews;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Reviews.Commands.CreateReview;
using AppointmentApplication.Application.Features.Reviews.Dtos;

using AppointmentApplication.Application.Features.Reviews.Queries.GetReviewByAppointmentId;
using AppointmentApplication.Application.Features.Reviews.Queries.GetReviewsByHealthCareFacilityId;
using AppointmentApplication.Application.Shared.Services;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/health-care-facilities/me/reviews")]
[Authorize(Roles = Roles.HealthCareFacility)]

public sealed class HealthCareFacilityReviewController : ApiController
{
    private readonly ISender _sender;
    private readonly LinkService _linkService;
    private readonly IUserContext _userContext;

    public HealthCareFacilityReviewController(ISender sender, LinkService linkService, IUserContext userContext)
    {
        _sender = sender;
        _linkService = linkService;
        _userContext = userContext;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginationResult<ExpandoObject>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [OutputCache(Duration = 60)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Reviews by Healthcare Facility Id.")]
    [EndpointDescription("Retrieves reviews for a healthcare facility with optional filtering and pagination.")]
    [EndpointName("GetReviewsByHealthCareFacilityId")]
    public async Task<IActionResult> GetReviewsByHealthCareFacilityId(
        Guid facilityId,
        [FromQuery] ReviewQueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetReviewsByHealthCareFacilityIdQuery(
                _userContext.UserId,
                queryParameters.Search,
                queryParameters.Page,
                queryParameters.PageSize,
                queryParameters.Sort,
                queryParameters.Fields,
                queryParameters.MinRating,
                queryParameters.MaxRating,
                queryParameters.FromDate,
                queryParameters.ToDate,
                queryParameters.DoctorId,
                queryParameters.PatientId),
            cancellationToken);

        return result.Match(
            response =>
            {
                var hasNextPage = response.Page < response.TotalPages;
                var hasPreviousPage = response.Page > 1;

                var links = CreateLinksForReviews(facilityId, queryParameters, hasNextPage, hasPreviousPage);

                var resource = new
                {
                    data = response.Items,
                    pagination = new
                    {
                        response.Page,
                        response.PageSize,
                        response.TotalCount,
                        response.TotalPages
                    },
                    links
                };

                return Ok(resource);
            },
            Problem);
    }

    private List<LinkDto> CreateLinksForReviews(Guid facilityId, ReviewQueryParameters parameters, bool hasNextPage, bool hasPreviousPage)
    {
        var links = new List<LinkDto>
        {
            _linkService.Create(nameof(GetReviewsByHealthCareFacilityId), "self", HttpMethods.Get, new
            {
                facilityId,
                page = parameters.Page,
                pageSize = parameters.PageSize,
                fields = parameters.Fields,
                search = parameters.Search,
                sort = parameters.Sort,
                minRating = parameters.MinRating,
                maxRating = parameters.MaxRating
            })
        };

        if (hasNextPage)
        {
            links.Add(_linkService.Create(nameof(GetReviewsByHealthCareFacilityId), "next-page", HttpMethods.Get, new
            {
                facilityId,
                page = parameters.Page + 1,
                pageSize = parameters.PageSize,
                fields = parameters.Fields,
                search = parameters.Search,
                sort = parameters.Sort,
                minRating = parameters.MinRating,
                maxRating = parameters.MaxRating
            }));
        }

        if (hasPreviousPage)
        {
            links.Add(_linkService.Create(nameof(GetReviewsByHealthCareFacilityId), "previous-page", HttpMethods.Get, new
            {
                facilityId,
                page = parameters.Page - 1,
                pageSize = parameters.PageSize,
                fields = parameters.Fields,
                search = parameters.Search,
                sort = parameters.Sort,
                minRating = parameters.MinRating,
                maxRating = parameters.MaxRating
            }));
        }

        return links;
    }

}