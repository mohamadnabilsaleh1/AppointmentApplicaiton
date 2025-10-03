using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Dtos.HealthCareFacilities;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.UpdateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Mappers;
using AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilities;
using AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilityById;
using AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilityByUserId;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Contracts.Requests;
using AppointmentApplication.Contracts.Requests.HealthCareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/health-care-facilities")]
public sealed class HealthCareFacilityController(ISender sender, LinkService linkService) : ApiController
{
    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(typeof(HealthcareFacilityDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Creates a new Health Care Facility.")]
    [EndpointDescription("Adds a new Health Care Facility to the system.")]
    [EndpointName("CreateHealthCareFacility")]
    public async Task<IActionResult> CreateHealthCareFacility([FromBody] CreateHealthcareFacilityRequest request, CancellationToken cancellationToken)
    {
        Result<HealthCareFacility> result = await sender.Send(
            new CreateHealthcareFacilityCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                request.Name,
                request.Type,
                request.Street,
                request.City,
                request.State,
                request.Country,
                request.ZipCode,
                request.GPSLatitude,
                request.GPSLongitude),
            cancellationToken);

        return result.Match(
            response =>
            {
                var dto = response.ToDto();
                var links = CreateLinks(response.Id.ToString(), null); // HATEOAS links

                var resource = new
                {
                    data = dto,
                    links
                };

                return CreatedAtRoute(
                    routeName: "GetHealthCareFacilityById",
                    routeValues: new { id = response.Id, apiVersion = "0.1" },
                    value: resource);
            },
            Problem);
    }

    [HttpGet("{id:guid}", Name = "GetHealthCareFacilityById")]
    [ProducesResponseType(typeof(HealthcareFacilityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Health Care Facility by Id.")]
    [EndpointDescription("Retrieves a single Health Care Facility.")]
    [EndpointName("GetHealthCareFacilityById")]
    public async Task<IActionResult> GetHealthCareFacilityById(Guid id, string? fields, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetHealthCareFacilityByIdQuery(id, fields), cancellationToken);

        return result.Match(
            facility =>
            {
                var links = CreateLinks(id.ToString(), fields); // HATEOAS links
                var resource = new
                {
                    data = facility,
                    links
                };
                return Ok(resource);
            },
            Problem);
    }
    [HttpGet("me", Name = "GetHealthCareFacilityByUserId")]
    [ProducesResponseType(typeof(HealthcareFacilityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Health Care Facility by User Id.")]
    [EndpointDescription("Retrieves a single Health Care Facility.")]
    [EndpointName("GetHealthCareFacilityByUserId")]
    public async Task<IActionResult> GetHealthCareFacilityByUserId(Guid id, string? fields, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetHealthCareFacilityByUserIdQuery(), cancellationToken);

        return result.Match(
            facility =>
            {
                var links = CreateLinks(id.ToString(), fields); // HATEOAS links
                var resource = new
                {
                    data = facility,
                    links
                };
                return Ok(resource);
            },
            Problem);
    }

    [HttpGet]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Health Care Facilities.")]
    [EndpointDescription("Retrieves health care facilities with optional search and pagination.")]
    [EndpointName("GetHealthCareFacilities")]
    [ProducesResponseType(typeof(PaginationResult<HealthcareFacilityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> GetHealthCareFacilities(
        [FromQuery] HealthCareFacilityQueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetHealthCareFacilityQuery(
                queryParameters.Search,
                queryParameters.Page,
                queryParameters.PageSize,
                queryParameters.Sort,
                queryParameters.Fields,
                queryParameters.Type,
                queryParameters.Street,
                queryParameters.City,
                queryParameters.State,
                queryParameters.Country,
                queryParameters.ZipCode,
                queryParameters.GPSLatitude,
                queryParameters.GPSLongitude,
                queryParameters.radiusKm),
            cancellationToken);

        return result.Match(
            response =>
            {
                var hasNextPage = response.Page < response.TotalPages;
                var hasPreviousPage = response.Page > 1;

                var links = CreateLinks(queryParameters, hasNextPage, hasPreviousPage);

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
    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.HealthCareFacility}")]
    [ProducesResponseType(typeof(HealthcareFacilityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Updates an existing Health Care Facility.")]
    [EndpointDescription("Updates the details of an existing Health Care Facility.")]
    [EndpointName("UpdateHealthCareFacility")]
    public async Task<IActionResult> UpdateHealthCareFacility(
    Guid id,
    [FromBody] UpdateHealthcareFacilityRequest request,
    CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new UpdateHealthcareFacilityCommand(
                id,
                request.Name,
                request.Street,
                request.City,
                request.State,
                request.Country,
                request.ZipCode,
                request.GPSLatitude,
                request.GPSLongitude),
            cancellationToken);

        return result.Match(
            response =>
            {
                var dto = response.ToDto();
                var links = CreateLinks(response.Id.ToString(), null);

                var resource = new
                {
                    data = dto,
                    links
                };

                return Ok(resource);
            },
            Problem);
    }

    private List<LinkDto> CreateLinks(HealthCareFacilityQueryParameters parameters, bool hasNextPage, bool hasPreviousPage)
    {
        List<LinkDto> links = new()
        {
            linkService.Create(nameof(GetHealthCareFacilities), "self", HttpMethods.Get, new
            {
                page = parameters.Page,
                pageSize = parameters.PageSize,
                fields = parameters.Fields,
                q = parameters.Search,
                sort = parameters.Sort
            }),
        };

        if (hasNextPage)
        {
            links.Add(linkService.Create(nameof(GetHealthCareFacilities), "next-page", HttpMethods.Get, new
            {
                page = parameters.Page + 1,
                pageSize = parameters.PageSize,
                fields = parameters.Fields,
                q = parameters.Search,
                sort = parameters.Sort
            }));
        }

        if (hasPreviousPage)
        {
            links.Add(linkService.Create(nameof(GetHealthCareFacilities), "previous-page", HttpMethods.Get, new
            {
                page = parameters.Page - 1,
                pageSize = parameters.PageSize,
                fields = parameters.Fields,
                q = parameters.Search,
                sort = parameters.Sort
            }));
        }

        return links;
    }

    private List<LinkDto> CreateLinks(string id, string? fields)
    {
        return new List<LinkDto>
        {
            linkService.Create(nameof(GetHealthCareFacilityById), "self", HttpMethods.Get, new { id, fields }),
            linkService.Create(nameof(CreateHealthCareFacility), "create", HttpMethods.Post),
            linkService.Create(nameof(UpdateHealthCareFacility), "update", HttpMethods.Put, new { id }),
            linkService.Create(nameof(GetHealthCareFacilityByUserId), "Get", HttpMethods.Get)

        };
    }
}
