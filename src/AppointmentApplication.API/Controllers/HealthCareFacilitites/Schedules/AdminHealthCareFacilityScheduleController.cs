using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilityByUserId;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Contracts.Requests.HealthCareFacilitites;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/health-care-facilities/me/schedules")]
[Authorize(Roles = Roles.HealthCareFacility)]
public sealed class AdminHealthCareFacilityScheduleController : ApiController
{
    private readonly ISender _sender;
    private readonly LinkService _linkService;

    public AdminHealthCareFacilityScheduleController(ISender sender, LinkService linkService)
    {
        _sender = sender;
        _linkService = linkService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Creates a new Health Care Facility Schedule")]
    [EndpointDescription("Adds a new schedule to the currently authenticated health care facility's availability.")]
    [EndpointName("AdminCreateHealthCareFacilitySchedule")]
    public async Task<IActionResult> CreateHealthCareFacilitySchedule(
        [FromBody] CreateHealthcareFacilityScheduleRequest request,
        CancellationToken cancellationToken)
    {
        // Implementation example:
        // var result = await _sender.Send(new CreateHealthCareFacilityScheduleCommand(request), cancellationToken);
        // return result.Match(
        //     schedule => {
        //         var links = CreateLinks(schedule.Id.ToString(), null);
        //         var resource = new { data = schedule, links };
        //         return CreatedAtRoute("GetHealthCareFacilityScheduleById", new { id = schedule.Id }, resource);
        //     },
        //     Problem);

        return Ok();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Health Care Facility Schedule by ID")]
    [EndpointDescription("Retrieves a specific schedule for the currently authenticated health care facility.")]
    [EndpointName("AdminGetHealthCareFacilityScheduleById")]
    public async Task<IActionResult> GetHealthCareFacilityScheduleById(Guid id, CancellationToken cancellationToken)
    {
        // Implementation example:
        // var result = await _sender.Send(new GetHealthCareFacilityScheduleByIdQuery(id), cancellationToken);
        // return result.Match(
        //     schedule => {
        //         var links = CreateLinks(id.ToString(), null);
        //         var resource = new { data = schedule, links };
        //         return Ok(resource);
        //     },
        //     Problem);

        return Ok();
    }

    [HttpGet]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Health Care Facility Schedules")]
    [EndpointDescription("Retrieves all schedules for the currently authenticated health care facility with pagination support.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [OutputCache(Duration = 60)]
    [EndpointName("AdminGetHealthCareFacilitySchedules")]
    public async Task<IActionResult> GetHealthCareFacilitySchedules(CancellationToken cancellationToken)
    {
        // Implementation example:
        // var result = await _sender.Send(new GetHealthCareFacilitySchedulesQuery(), cancellationToken);
        // return result.Match(
        //     paginationResult => {
        //         var links = CreatePaginationLinks(paginationResult);
        //         var resource = new {
        //             data = paginationResult.Items,
        //             pagination = new {
        //                 paginationResult.Page,
        //                 paginationResult.PageSize,
        //                 paginationResult.TotalCount,
        //                 paginationResult.TotalPages
        //             },
        //             links
        //         };
        //         return Ok(resource);
        //     },
        //     Problem);

        return Ok();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Updates a Health Care Facility Schedule")]
    [EndpointDescription("Modifies an existing schedule for the currently authenticated health care facility.")]
    [EndpointGroupName("AdminUpdateHealthCareFacilitySchedule")]
    public async Task<IActionResult> UpdateHealthCareFacilitySchedule(
        Guid id,
        [FromBody] UpdateHealthcareFacilityScheduleRequest request,
        CancellationToken cancellationToken)
    {
        // Implementation example:
        // var result = await _sender.Send(new UpdateHealthCareFacilityScheduleCommand(id, request), cancellationToken);
        // return result.Match(
        //     schedule => {
        //         var links = CreateLinks(id.ToString(), null);
        //         var resource = new { data = schedule, links };
        //         return Ok(resource);
        //     },
        //     Problem);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Deletes a Health Care Facility Schedule")]
    [EndpointDescription("Removes a specific schedule from the currently authenticated health care facility.")]
    [EndpointName("AdminDeleteHealthCareFacilitySchedule")]
    public async Task<IActionResult> DeleteHealthCareFacilitySchedule(Guid id, CancellationToken cancellationToken)
    {
        // Implementation example:
        // var result = await _sender.Send(new DeleteHealthCareFacilityScheduleCommand(id), cancellationToken);
        // return result.Match<IActionResult>(_ => NoContent(), Problem);

        return NoContent();
    }

    private List<LinkDto> CreateLinks(string id, string? fields)
    {
        return new List<LinkDto>
        {
            _linkService.Create("AdminGetHealthCareFacilityScheduleById", "self", HttpMethods.Get, new { id, fields }),
            _linkService.Create("AdminCreateHealthCareFacilitySchedule", "create", HttpMethods.Post),
            _linkService.Create("AdminUpdateHealthCareFacilitySchedule", "update", HttpMethods.Put, new { id }),
            _linkService.Create("AdminDeleteHealthCareFacilitySchedule", "delete", HttpMethods.Delete, new { id }),
            _linkService.Create("AdminGetHealthCareFacilitySchedules", "all", HttpMethods.Get)
        };
    }

    private List<LinkDto> CreatePaginationLinks(PaginationResult<object> paginationResult)
    {
        var links = new List<LinkDto>
        {
            _linkService.Create("GetHealthCareFacilitySchedules", "self", HttpMethods.Get, new
            {
                page = paginationResult.Page,
                pageSize = paginationResult.PageSize
            })
        };

        if (paginationResult.Page < paginationResult.TotalPages)
        {
            links.Add(_linkService.Create("GetHealthCareFacilitySchedules", "next-page", HttpMethods.Get, new
            {
                page = paginationResult.Page + 1,
                pageSize = paginationResult.PageSize
            }));
        }

        if (paginationResult.Page > 1)
        {
            links.Add(_linkService.Create("GetHealthCareFacilitySchedules", "previous-page", HttpMethods.Get, new
            {
                page = paginationResult.Page - 1,
                pageSize = paginationResult.PageSize
            }));
        }

        return links;
    }
}