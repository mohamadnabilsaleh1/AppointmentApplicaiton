using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Domain.Users;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/health-care-facility/{healthcareId:guid}/schedule-exceptions")]
[Authorize]
public sealed class HealthCareFacilityScheduleExceptionController : ApiController
{
    private readonly ISender _sender;
    private readonly LinkService _linkService;

    public HealthCareFacilityScheduleExceptionController(ISender sender, LinkService linkService)
    {
        _sender = sender;
        _linkService = linkService;
    }

    // GET all schedule exceptions for a specific healthcare facility
    [HttpGet]
    [MapToApiVersion("0.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [OutputCache(Duration = 60)]
    [EndpointName("GetHealthCareFacilityScheduleExceptions")]
    [EndpointSummary("Retrieve all schedule exceptions for a healthcare facility")]
    [EndpointDescription("Fetches all schedule exceptions associated with the specified healthcare facility.")]
    public async Task<IActionResult> GetHealthCareFacilityScheduleException(
        Guid healthcareId,
        CancellationToken cancellationToken)
    {
        // Implementation placeholder
        return Ok();
    }

    // GET schedule exception by Id
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointName("GetHealthCareFacilityScheduleExceptionById")]
    [EndpointSummary("Retrieve a schedule exception by ID")]
    [EndpointDescription("Fetches a specific schedule exception for the specified healthcare facility by its unique ID.")]
    public async Task<IActionResult> GetHealthCareFacilityScheduleExceptionById(
        Guid healthcareId,
        Guid id,
        CancellationToken cancellationToken)
    {
        // Implementation placeholder
        return Ok();
    }

    private List<LinkDto> CreateLinks(Guid healthcareId, Guid? id = null)
    {
        var links = new List<LinkDto>
        {
            _linkService.Create(nameof(GetHealthCareFacilityScheduleException), "self", HttpMethods.Get, new { healthcareId }),
            _linkService.Create(nameof(GetHealthCareFacilityScheduleExceptionById), "self", HttpMethods.Get, new { healthcareId, id })
        };
        return links;
    }
}
