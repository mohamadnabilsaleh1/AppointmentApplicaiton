using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Doctors.ScheduleExceptions.Mappers;

using AppointmentApplication.Contracts.Requests.Doctors.ScheduleExceptions;
using AppointmentApplication.Domain.Users;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/doctors/me/schedule-exceptions")]
[Authorize(Roles = Roles.Doctor)]
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
    [EndpointName("AdminCreateDoctorScheduleException")]
    [EndpointSummary("Creates a new schedule exception")]
    [EndpointDescription("Adds a new schedule exception for the currently authenticated doctor.")]
    public async Task<IActionResult> CreateDoctorScheduleException(
        [FromBody] CreateDoctorScheduleExceptionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new CreateScheduleExceptionCommand(
                _userContext.UserId,
                request.Date,
                request.StartTime,
                request.EndTime,
                request.Status,
                request.Reason),
            cancellationToken);

        return result.Match(
            schedule =>
            {
                var links = CreateLinks(schedule.Id.ToString(), null);
                var resource = new { data = schedule, links };

                return CreatedAtAction(
                    nameof(GetDoctorScheduleExceptionById),
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
    [EndpointName("AdminGetDoctorScheduleExceptionById")]
    [EndpointSummary("Retrieve a schedule exception by ID")]
    [EndpointDescription("Fetches a specific schedule exception for the currently authenticated doctor.")]
    public async Task<IActionResult> GetDoctorScheduleExceptionById(
        Guid id,
        string? fields,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetScheduleExceptionByUserIdQuery(_userContext.UserId, id),
            cancellationToken);

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
    [EndpointName("AdminGetDoctorScheduleExceptions")]
    [EndpointSummary("Retrieve all schedule exceptions")]
    [EndpointDescription("Fetches all schedule exceptions for the currently authenticated doctor.")]
    public async Task<IActionResult> GetDoctorScheduleExceptions(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetScheduleExceptionsByUserIdQuery(_userContext.UserId),
            cancellationToken);

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
    [EndpointName("AdminUpdateDoctorScheduleException")]
    [EndpointSummary("Updates a schedule exception")]
    [EndpointDescription("Modifies an existing schedule exception for the currently authenticated doctor.")]
    public async Task<IActionResult> UpdateDoctorScheduleException(
        Guid id,
        [FromBody] UpdateDoctorScheduleExceptionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new UpdateScheduleExceptionCommand(
                _userContext.UserId,
                id,
                request.Date,
                request.StartTime,
                request.EndTime,
                request.Status,
                request.Reason),
            cancellationToken);

        return result.Match<IActionResult>(_ => NoContent(), Problem);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointName("AdminDeleteDoctorScheduleException")]
    [EndpointSummary("Deletes a schedule exception")]
    [EndpointDescription("Removes a specific schedule exception for the currently authenticated doctor.")]
    public async Task<IActionResult> DeleteDoctorScheduleException(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new DeleteScheduleExceptionCommand(_userContext.UserId, id),
            cancellationToken);

        return result.Match<IActionResult>(_ => NoContent(), Problem);
    }

    private List<LinkDto> CreateLinks(string id, string? fields)
    {
        return new List<LinkDto>
        {
            _linkService.Create(nameof(GetDoctorScheduleExceptionById), "self", HttpMethods.Get, new { id, fields }),
            _linkService.Create(nameof(CreateDoctorScheduleException), "create", HttpMethods.Post),
            _linkService.Create(nameof(UpdateDoctorScheduleException), "update", HttpMethods.Put, new { id }),
            _linkService.Create(nameof(DeleteDoctorScheduleException), "delete", HttpMethods.Delete, new { id }),
            _linkService.Create(nameof(GetDoctorScheduleExceptions), "all", HttpMethods.Get)
        };
    }
}
