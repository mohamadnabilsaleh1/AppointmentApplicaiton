using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Contracts.Requests.HealthCareFacilitites.ScheduleExceptions;
using AppointmentApplication.Domain.Users;

using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/health-care-facility/me/schedule-exceptions")]
[Authorize(Roles = Roles.HealthCareFacility)]
public sealed class AdminHealthCareFacilityScheduleExceptionController : ApiController
{
    private readonly ISender _sender;
    private readonly LinkService _linkService;

    public AdminHealthCareFacilityScheduleExceptionController(ISender sender, LinkService linkService)
    {
        _sender = sender;
        _linkService = linkService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    public async Task<IActionResult> CreateHealthCareFacilityScheduleException(
        [FromBody] CreateHealthCareFacilityScheduleExceptionRequest request,
        CancellationToken cancellationToken)
    {
        return NoContent();
    }

    [HttpGet("{id:guid}", Name = "admin-get-health-care-facility-schedule-exception-by-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    public async Task<IActionResult> GetHealthCareFacilityScheduleExceptionById(
        Guid id,
        string? fields,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet(Name = "admin-get-health-care-facility-schedule-exceptions")]
    [MapToApiVersion("0.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> GetHealthCareFacilityScheduleExceptions(
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpPut("{id:guid}", Name = "update-health-care-facility-schedule-exception")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    public async Task<IActionResult> UpdateHealthCareFacilityScheduleException(
        Guid id,
        [FromBody] UpdateHealthCareFacilityScheduleExceptionRequest request,
        CancellationToken cancellationToken)
    {
        return NoContent();
    }

    [HttpDelete("{id:guid}", Name = "admin-delete-health-care-facility-schedule-exception")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    public async Task<IActionResult> DeleteHealthCareFacilityScheduleException(
        Guid id,
        CancellationToken cancellationToken)
    {
        return NoContent();
    }

    private List<LinkDto> CreateLinks(string id, string? fields)
    {
        return new List<LinkDto>
        {
            _linkService.Create(nameof(GetHealthCareFacilityScheduleExceptionById), "self", HttpMethods.Get, new { id, fields }),
            _linkService.Create(nameof(CreateHealthCareFacilityScheduleException), "create", HttpMethods.Post),
            _linkService.Create(nameof(UpdateHealthCareFacilityScheduleException), "update", HttpMethods.Put, new { id }),
            _linkService.Create(nameof(DeleteHealthCareFacilityScheduleException), "delete", HttpMethods.Delete, new { id }),
            _linkService.Create(nameof(GetHealthCareFacilityScheduleExceptions), "all", HttpMethods.Get)
        };
    }
}
