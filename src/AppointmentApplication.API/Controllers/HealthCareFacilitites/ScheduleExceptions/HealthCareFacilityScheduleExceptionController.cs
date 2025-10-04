using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Domain.Users;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/healthcarefacility/{healthcareId:guid}/schedules")]
[Authorize(Roles = Roles.HealthCareFacility)]
public sealed class HealthCareFacilityScheduleController : ApiController
{
    private readonly ISender _sender;
    private readonly LinkService _linkService;

    public HealthCareFacilityScheduleController(ISender sender, LinkService linkService)
    {
        _sender = sender;
        _linkService = linkService;
    }

    // GET all schedules for a specific healthcare facility
    [HttpGet(Name = "GetHealthCareFacilitySchedules")]
    [MapToApiVersion("0.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> GetHealthCareFacilitySchedules(
        Guid healthcareId,
        CancellationToken cancellationToken)
    {
        // هنا يمكنك استدعاء _sender.Send() لجلب البيانات من DB
        return Ok(); // placeholder
    }

    // GET schedule by Id
    [HttpGet("{id:guid}", Name = "GetHealthCareFacilityScheduleById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    public async Task<IActionResult> GetHealthCareFacilityScheduleById(
        Guid healthcareId,
        Guid id,
        CancellationToken cancellationToken)
    {
        // هنا يمكنك استدعاء _sender.Send() لجلب البيانات من DB حسب Id
        return Ok(); // placeholder
    }

    private List<LinkDto> CreateLinks(Guid healthcareId, Guid? id = null)
    {
        var links = new List<LinkDto>
        {
            _linkService.Create(nameof(GetHealthCareFacilitySchedules), "self", HttpMethods.Get, new { healthcareId }),
            _linkService.Create(nameof(GetHealthCareFacilityScheduleById), "self", HttpMethods.Get, new { healthcareId, id })
        };
        return links;
    }
}
