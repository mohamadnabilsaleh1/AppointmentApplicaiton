using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Doctors.Schedules.Commands;
using AppointmentApplication.Application.Features.Doctors.Schedules.Queries;

using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Contracts.Requests.Doctors;
using AppointmentApplication.Contracts.Requests.Doctors.Schedules;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[ApiController]
[Route("api/doctors/me/schedules")]
[Authorize(Roles = Roles.Doctor)]
public sealed class AdminDoctorScheduleController : ApiController
{
    private readonly ISender _sender;
    private readonly LinkService _linkService;
    private readonly IUserContext _userContext;

    public AdminDoctorScheduleController(ISender sender, LinkService linkService, IUserContext userContext)
    {
        _sender = sender;
        _linkService = linkService;
        _userContext = userContext;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Creates a new Doctor Schedule")]
    [EndpointDescription("Adds a new schedule to the currently authenticated doctor's availability.")]
    [EndpointName("AdminCreateDoctorSchedule")]
    public async Task<IActionResult> CreateDoctorSchedule(
        [FromBody] CreateDoctorScheduleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new CreateScheduleCommand(
                _userContext.UserId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime,
                request.Status,
                request.Note),
            cancellationToken);

        return result.Match(
            schedule =>
            {
                var links = CreateLinks(schedule.Id.ToString(), null);
                var resource = new { data = schedule, links };

                return CreatedAtAction(
                    nameof(GetDoctorScheduleById),
                    new { id = schedule.Id },
                    resource);
            },
            Problem);
    }

    [HttpGet("{id:guid}", Name = "GetDoctorScheduleById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Doctor Schedule by ID")]
    [EndpointDescription("Retrieves a specific schedule for the currently authenticated doctor.")]
    [EndpointName("AdminGetDoctorScheduleById")]
    public async Task<IActionResult> GetDoctorScheduleById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetScheduleByUserIdQuery(_userContext.UserId, id), cancellationToken);
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
    [EndpointSummary("Get Doctor Schedules")]
    [EndpointDescription("Retrieves all schedules for the currently authenticated doctor with pagination support.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [OutputCache(Duration = 60)]
    [EndpointName("AdminGetDoctorSchedules")]
    public async Task<IActionResult> GetDoctorSchedules(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetSchedulesByUserIdQuery(_userContext.UserId), cancellationToken);
        return result.Match(
            schedules =>
            {
                var resource = new { data = schedules };
                return Ok(resource);
            },
            Problem);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Updates a Doctor Schedule")]
    [EndpointDescription("Modifies an existing schedule for the currently authenticated doctor.")]
    [EndpointGroupName("AdminUpdateDoctorSchedule")]
    public async Task<IActionResult> UpdateDoctorSchedule(
        Guid id,
        [FromBody] UpdateDoctorScheduleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new UpdateScheduleCommand(
                _userContext.UserId,
                id,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime,
                request.Status,
                request.IsAvailable,
                request.Note),
            cancellationToken);

        return result.Match<IActionResult>(_ => NoContent(), Problem);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Deletes a Doctor Schedule")]
    [EndpointDescription("Removes a specific schedule from the currently authenticated doctor.")]
    [EndpointName("AdminDeleteDoctorSchedule")]
    public async Task<IActionResult> DeleteDoctorSchedule(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteScheduleCommand(_userContext.UserId, id), cancellationToken);
        return result.Match<IActionResult>(_ => NoContent(), Problem);
    }

    private List<LinkDto> CreateLinks(string id, string? fields)
    {
        return new List<LinkDto>
        {
            _linkService.Create(nameof(GetDoctorScheduleById), "self", HttpMethods.Get, new { id, fields }),
            _linkService.Create(nameof(CreateDoctorSchedule), "create", HttpMethods.Post),
            _linkService.Create(nameof(UpdateDoctorSchedule), "update", HttpMethods.Put, new { id }),
            _linkService.Create(nameof(DeleteDoctorSchedule), "delete", HttpMethods.Delete, new { id }),
            _linkService.Create(nameof(GetDoctorSchedules), "all", HttpMethods.Get)
        };
    }

    private List<LinkDto> CreatePaginationLinks(PaginationResult<object> paginationResult)
    {
        var links = new List<LinkDto>
        {
            _linkService.Create("GetDoctorSchedules", "self", HttpMethods.Get, new
            {
                page = paginationResult.Page,
                pageSize = paginationResult.PageSize
            })
        };

        if (paginationResult.Page < paginationResult.TotalPages)
        {
            links.Add(_linkService.Create("GetDoctorSchedules", "next-page", HttpMethods.Get, new
            {
                page = paginationResult.Page + 1,
                pageSize = paginationResult.PageSize
            }));
        }

        if (paginationResult.Page > 1)
        {
            links.Add(_linkService.Create("GetDoctorSchedules", "previous-page", HttpMethods.Get, new
            {
                page = paginationResult.Page - 1,
                pageSize = paginationResult.PageSize
            }));
        }

        return links;
    }
}
