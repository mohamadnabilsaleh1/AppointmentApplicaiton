using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Dtos.HealthCareFacilities;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.ActivateHealthcareFacilityById;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.AddDescription;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.DeactivateHealthcareFacilityById;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.UpdateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.UpdateHealthcareFacilityByUserId;
using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Mappers;
using AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilities;
using AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilityById;
using AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilityByUserId;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Contracts.Requests;
using AppointmentApplication.Contracts.Requests.Doctors;
using AppointmentApplication.Contracts.Requests.HealthCareFacilities;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;
//AddDescriptionCommand
[Route("api/health-care-facilities")]
public sealed class HealthCareFacilityController : ApiController
{
    private readonly ISender _sender;
    private readonly LinkService _linkService;
    private readonly IUserContext _userContext; // للحصول على الـ UserId الحالي

    public HealthCareFacilityController(ISender sender, LinkService linkService, IUserContext userContext)
    {
        _sender = sender;
        _linkService = linkService;
        _userContext = userContext;
    }

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
        var result = await _sender.Send(
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

    [HttpGet]
    [ProducesResponseType(typeof(PaginationResult<HealthcareFacilityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [OutputCache(Duration = 60)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Health Care Facilities.")]
    [EndpointDescription("Retrieves health care facilities with optional search and pagination.")]
    [EndpointName("GetHealthCareFacilities")]
    public async Task<IActionResult> GetHealthCareFacilities(
        [FromQuery] HealthCareFacilityQueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
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
        var result = await _sender.Send(new GetHealthCareFacilityByIdQuery(id, fields), cancellationToken);

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

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{Roles.Admin}")]
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
        var result = await _sender.Send(
            new UpdateHealthcareFacilityByIdCommand(
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
            response => NoContent(),
            Problem);
    }

    [HttpGet("me")]
    [Authorize(Roles = Roles.HealthCareFacility)]
    [ProducesResponseType(typeof(HealthcareFacilityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Health Care Facility of logged-in user.")]
    [EndpointDescription("Retrieves the Health Care Facility associated with the authenticated user.")]
    [EndpointName("GetHealthCareFacilityMe")]
    public async Task<IActionResult> GetHealthCareFacilityMe(string? fields, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetHealthCareFacilityByUserIdQuery(_userContext.UserId), cancellationToken);

        return result.Match(
            facility =>
            {
                var links = CreateLinks(facility.Id.ToString(), fields); // HATEOAS links
                var resource = new
                {
                    data = facility,
                    links
                };
                return Ok(resource);
            },
            Problem);
    }

    [HttpPut("me")]
    [Authorize(Roles = Roles.HealthCareFacility)]
    [ProducesResponseType(typeof(HealthcareFacilityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Update Health Care Facility of logged-in user.")]
    [EndpointDescription("Updates the Health Care Facility data associated with the authenticated user.")]
    [EndpointName("UpdateHealthCareFacilityMe")]
    public async Task<IActionResult> UpdateHealthCareFacilityMe(
        [FromBody] UpdateHealthcareFacilityRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new UpdateHealthcareFacilityByUserIdCommand(
                _userContext.UserId,
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
            response => NoContent(),
            Problem);
    }

    [HttpPatch("{id:guid}/activate")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Activate a Health Care Facility.")]
    [EndpointDescription("Sets the IsActive flag of a Health Care Facility to true.")]
    [EndpointName("ActivateHealthCareFacility")]
    public async Task<IActionResult> ActivateHealthCareFacility(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ActivateHealthcareFacilityByIdCommand(id), cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPatch("{id:guid}/deactivate")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Deactivate a Health Care Facility.")]
    [EndpointDescription("Sets the IsActive flag of a Health Care Facility to false.")]
    [EndpointName("DeactivateHealthCareFacility")]
    public async Task<IActionResult> DeactivateHealthCareFacility(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeactivateHealthcareFacilityByIdCommand(id), cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
    [HttpPut("me/description")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EndpointName("UpdateHealthCareFacilityDescription")]
    [EndpointSummary("Update Health Care Facility description.")]
    [EndpointDescription("Updates the description for a specific doctor.")]
    public async Task<IActionResult> UpdateDescription(
Guid doctorId,
[FromBody] UpdateDescriptionRequest request,
CancellationToken cancellationToken)
    {
        var command = new AddDescriptionHealthCareFacilityCommand(_userContext.UserId, request.Description);
        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    private List<LinkDto> CreateLinks(HealthCareFacilityQueryParameters parameters, bool hasNextPage, bool hasPreviousPage)
    {
        var links = new List<LinkDto>
        {
            _linkService.Create(nameof(GetHealthCareFacilities), "self", HttpMethods.Get, new
            {
                page = parameters.Page,
                pageSize = parameters.PageSize,
                fields = parameters.Fields,
                q = parameters.Search,
                sort = parameters.Sort
            })
        };

        if (hasNextPage)
        {
            links.Add(_linkService.Create(nameof(GetHealthCareFacilities), "next-page", HttpMethods.Get, new
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
            links.Add(_linkService.Create(nameof(GetHealthCareFacilities), "previous-page", HttpMethods.Get, new
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
        var links = new List<LinkDto>
    {
        _linkService.Create(nameof(GetHealthCareFacilityById), "self", HttpMethods.Get, new { id, fields }),
        _linkService.Create(nameof(GetHealthCareFacilities), "all", HttpMethods.Get),
        _linkService.Create(nameof(CreateHealthCareFacility), "create", HttpMethods.Post),
        _linkService.Create(nameof(UpdateHealthCareFacility), "update", HttpMethods.Put, new { id }),
        _linkService.Create(nameof(DeactivateHealthCareFacility), "deactivate", HttpMethods.Patch, new { id }),
        _linkService.Create(nameof(ActivateHealthCareFacility), "activate", HttpMethods.Patch, new { id }),
        _linkService.Create(nameof(GetHealthCareFacilityMe), "get-me", HttpMethods.Get),
        _linkService.Create(nameof(UpdateHealthCareFacilityMe), "update-me", HttpMethods.Put)
    };

        return links;
    }
}