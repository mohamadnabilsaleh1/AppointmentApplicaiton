using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilityByUserId;
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
public sealed class HealthCareFacilityScheduleController(
    ISender sender,
    LinkService linkService) : ApiController
{
    // POST: create schedule
    [HttpPost(Name = "create-health-care-facility-schedule")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Creates a new Health Care Facility Schedule.")]
    [EndpointDescription("Adds a new Health Care Facility Schedule to the logged-in facility.")]
    public async Task<IActionResult> CreateHealthCareFacilitySchedule(
        [FromBody] CreateHealthcareFacilityScheduleRequest request,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    // GET by Id
    [HttpGet("{id:guid}", Name = "get-health-care-facility-schedule-by-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Health Care Facility Schedule by Id.")]
    [EndpointDescription("Retrieves a single Health Care Facility Schedule of the logged-in facility.")]
    public async Task<IActionResult> GetHealthCareFacilityScheduleById(Guid id, CancellationToken cancellationToken)
    {
        return Ok();
    }

    // GET all schedules for logged-in facility
    [HttpGet(Name = "get-health-care-facility-schedules")]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Health Care Facility Schedules.")]
    [EndpointDescription("Retrieves all schedules for the logged-in facility with optional filtering and pagination.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> GetHealthCareFacilitySchedules(CancellationToken cancellationToken)
    {
        return Ok();
    }

    // PUT: update schedule
    [HttpPut("{id:guid}", Name = "update-health-care-facility-schedule")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Updates an existing Health Care Facility Schedule.")]
    [EndpointDescription("Updates the details of an existing Health Care Facility Schedule for the logged-in facility.")]
    public async Task<IActionResult> UpdateHealthCareFacilitySchedule(
        Guid id,
        [FromBody] UpdateHealthcareFacilityScheduleRequest request,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    // DELETE schedule
    [HttpDelete("{id:guid}", Name = "delete-health-care-facility-schedule")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Deletes a Health Care Facility Schedule.")]
    [EndpointDescription("Removes a Health Care Facility Schedule for the logged-in facility.")]
    public async Task<IActionResult> DeleteHealthCareFacilitySchedule(Guid id, CancellationToken cancellationToken)
    {
        return Ok();
    }

    private List<LinkDto> CreateLinks(string id, string? fields)
    {
        return new List<LinkDto>
        {
            linkService.Create("get-health-care-facility-schedule-by-id", "self", HttpMethods.Get, new { id, fields }),
            linkService.Create("create-health-care-facility-schedule", "create", HttpMethods.Post),
            linkService.Create("update-health-care-facility-schedule", "update", HttpMethods.Put, new { id }),
            linkService.Create("delete-health-care-facility-schedule", "delete", HttpMethods.Delete, new { id }),
            linkService.Create("get-health-care-facility-schedules", "all", HttpMethods.Get)
        };
    }
}
