using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Features.Doctors.ScheduleExceptions.Mappers;
using AppointmentApplication.Domain.Users;

using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/doctors/{facilityId:guid}/schedule-exceptions")]
public sealed class DoctorScheduleExceptionController : ApiController
{
    private readonly ISender _sender;
    private readonly LinkService _linkService;

    public DoctorScheduleExceptionController(ISender sender, LinkService linkService)
    {
        _sender = sender;
        _linkService = linkService;
    }

    [HttpGet]
    [MapToApiVersion("0.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [OutputCache(Duration = 60)]
    [EndpointName("DoctorGetHealthCareFacilityScheduleExceptions")]
    [EndpointSummary("Retrieve all schedule exceptions for a healthcare facility")]
    [EndpointDescription("Fetches all schedule exceptions associated with the specified healthcare facility.")]
    public async Task<IActionResult> GetHealthCareFacilityScheduleExceptions(
        Guid facilityId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetScheduleExceptionsByIdQuery(facilityId), cancellationToken);

        return result.Match(
            schedules =>
            {
                var resource = new { data = schedules };
                return Ok(resource);
            },
            Problem);
    }

    [HttpGet("{id:guid}")]
    [MapToApiVersion("0.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EndpointName("DoctorGetHealthCareFacilityScheduleExceptionById")]
    [EndpointSummary("Retrieve a schedule exception by ID")]
    [EndpointDescription("Fetches a specific schedule exception for the specified healthcare facility by its unique ID.")]
    public async Task<IActionResult> GetHealthCareFacilityScheduleExceptionsById(
        Guid facilityId,
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetScheduleExceptionByIdQuery(facilityId, id), cancellationToken);

        return result.Match(
            schedule =>
            {
                var links = CreateLinks(facilityId, id);
                var resource = new { data = schedule, links };
                return Ok(resource);
            },
            Problem);
    }

    private List<LinkDto> CreateLinks(Guid facilityId, Guid? scheduleId = null)
    {
        return new List<LinkDto>
        {
            _linkService.Create(nameof(GetHealthCareFacilityScheduleExceptions), "all", HttpMethods.Get, new { facilityId }),
            _linkService.Create(nameof(GetHealthCareFacilityScheduleExceptionsById), "self", HttpMethods.Get, new { facilityId, id = scheduleId })
        };
    }
}
