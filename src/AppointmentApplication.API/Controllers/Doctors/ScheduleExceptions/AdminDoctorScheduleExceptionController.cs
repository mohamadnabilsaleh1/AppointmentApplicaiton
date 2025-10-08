using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Commands;
using AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Queries;
using AppointmentApplication.Contracts.Requests.HealthCareFacilitites.ScheduleExceptions;
using AppointmentApplication.Domain.Users;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/doctors/me/schedule-exceptions")]
[Authorize(Roles = Roles.HealthCareFacility)]
public sealed class AdminDoctorScheduleExceptionController : ApiController
{
    private readonly ISender _sender;
    private readonly LinkService _linkService;
    private readonly IUserContext _userContext;
    public AdminDoctorScheduleExceptionController(ISender sender, LinkService linkService, IUserContext userContext)
    {
        _sender = sender;
        _linkService = linkService;
        _userContext = userContext;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointName("AdminCreateHealthCareFacilityScheduleException")]
    [EndpointSummary("Creates a new schedule exception")]
    [EndpointDescription("Adds a new schedule exception for the currently authenticated health care facility.")]
    public async Task<IActionResult> CreateHealthCareFacilityScheduleException(
        [FromBody] CreateHealthCareFacilityScheduleExceptionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new CreateScheduleExceptionCommand(_userContext.UserId, request.Date, request.StartTime, request.EndTime, request.Status, request.Reason), cancellationToken);

        return result.Match(
            schedule =>
            {
                var links = CreateLinks(schedule.Id.ToString(), null);
                var resource = new { data = schedule, links };

                // ✅ الحل: استخدام CreatedAtAction بدلاً من CreatedAtRoute
                return CreatedAtAction(
                    nameof(GetHealthCareFacilityScheduleExceptionById),
                    new { id = schedule.Id },
                    resource);
            },
            Problem);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointName("AdminGetHealthCareFacilityScheduleExceptionById")]
    [EndpointSummary("Retrieve a schedule exception by ID")]
    [EndpointDescription("Fetches a specific schedule exception for the currently authenticated health care facility.")]
    public async Task<IActionResult> GetHealthCareFacilityScheduleExceptionById(
        Guid id,
        string? fields,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetScheduleExceptionByUserIdQuery(_userContext.UserId, id), cancellationToken);
        return result.Match(
            schedule =>
            {
                var links = CreateLinks(id.ToString(), null);
                var resource = new { data = schedule, links };
                return Ok(resource);
            },
            Problem);
    }

    [HttpGet]
    [MapToApiVersion("0.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [OutputCache(Duration = 60)]
    [EndpointName("AdminGetHealthCareFacilityScheduleExceptions")]
    [EndpointSummary("Retrieve all schedule exceptions")]
    [EndpointDescription("Fetches all schedule exceptions for the currently authenticated health care facility.")]
    public async Task<IActionResult> GetHealthCareFacilityScheduleExceptions(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetScheduleExceptionsByUserIdQuery(_userContext.UserId), cancellationToken);
        return result.Match(
            schedules =>
            {
                var resource = new { data = schedules };
                return Ok(resource);
            },
            Problem);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointName("AdminUpdateHealthCareFacilityScheduleException")]
    [EndpointSummary("Updates a schedule exception")]
    [EndpointDescription("Modifies an existing schedule exception for the currently authenticated health care facility.")]
    public async Task<IActionResult> UpdateHealthCareFacilityScheduleException(
        Guid id,
        [FromBody] UpdateHealthCareFacilityScheduleExceptionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new UpdateScheduleExceptionCommand(_userContext.UserId, id, request.Date, request.StartTime, request.EndTime, request.Status, request.Reason), cancellationToken);
        return result.Match<IActionResult>(_ => NoContent(), Problem);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointName("AdminDeleteHealthCareFacilityScheduleException")]
    [EndpointSummary("Deletes a schedule exception")]
    [EndpointDescription("Removes a specific schedule exception for the currently authenticated health care facility.")]
    public async Task<IActionResult> DeleteHealthCareFacilityScheduleException(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteScheduleExceptionCommand(_userContext.UserId, id), cancellationToken);
        return result.Match<IActionResult>(_ => NoContent(), Problem);
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
