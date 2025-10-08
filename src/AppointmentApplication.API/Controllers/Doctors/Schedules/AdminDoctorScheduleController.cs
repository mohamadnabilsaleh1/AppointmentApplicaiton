using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilityByUserId;
using AppointmentApplication.Application.HealthcareFacilities.Schedules.Commands;
using AppointmentApplication.Application.HealthcareFacilities.Schedules.Queries;
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

[ApiController]
[Route("api/health-care-facilities/me/schedules")]
[Authorize(Roles = Roles.HealthCareFacility)]
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
    [EndpointSummary("Creates a new Health Care Facility Schedule")]
    [EndpointDescription("Adds a new schedule to the currently authenticated health care facility's availability.")]
    [EndpointName("AdminCreateHealthCareFacilitySchedule")]
    public async Task<IActionResult> CreateHealthCareFacilitySchedule(
        [FromBody] CreateHealthcareFacilityScheduleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new CreateScheduleCommand(_userContext.UserId, request.DayOfWeek, request.StartTime, request.EndTime, request.Status, request.Note), cancellationToken);

        return result.Match(
            schedule =>
            {
                var links = CreateLinks(schedule.Id.ToString(), null);
                var resource = new { data = schedule, links };

                // ✅ الحل: استخدام CreatedAtAction بدلاً من CreatedAtRoute
                return CreatedAtAction(
                    nameof(GetHealthCareFacilityScheduleById),
                    new { id = schedule.Id },
                    resource);
            },
            Problem);
    }

    [HttpGet("{id:guid}", Name = "GetHealthCareFacilityScheduleById")] // ✅ إضافة Name للـ Route
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Health Care Facility Schedule by ID")]
    [EndpointDescription("Retrieves a specific schedule for the currently authenticated health care facility.")]
    [EndpointName("AdminGetHealthCareFacilityScheduleById")]
    public async Task<IActionResult> GetHealthCareFacilityScheduleById(Guid id, CancellationToken cancellationToken)
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
    [EndpointSummary("Get Health Care Facility Schedules")]
    [EndpointDescription("Retrieves all schedules for the currently authenticated health care facility with pagination support.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [OutputCache(Duration = 60)]
    [EndpointName("AdminGetHealthCareFacilitySchedules")]
    public async Task<IActionResult> GetHealthCareFacilitySchedules(CancellationToken cancellationToken)
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
    [EndpointSummary("Updates a Health Care Facility Schedule")]
    [EndpointDescription("Modifies an existing schedule for the currently authenticated health care facility.")]
    [EndpointGroupName("AdminUpdateHealthCareFacilitySchedule")]
    public async Task<IActionResult> UpdateHealthCareFacilitySchedule(
        Guid id,
        [FromBody] UpdateHealthcareFacilityScheduleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new UpdateScheduleCommand(_userContext.UserId, id, request.DayOfWeek, request.StartTime, request.EndTime, request.Status, request.IsAvailable, request.Note), cancellationToken);
        return result.Match<IActionResult>(_ => NoContent(), Problem);
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
        var result = await _sender.Send(new DeleteScheduleCommand(_userContext.UserId, id), cancellationToken);
        return result.Match<IActionResult>(_ => NoContent(), Problem);
    }

    private List<LinkDto> CreateLinks(string id, string? fields)
    {
        return new List<LinkDto>
        {
            _linkService.Create(nameof(GetHealthCareFacilityScheduleById), "self", HttpMethods.Get, new { id, fields }),
            _linkService.Create(nameof(CreateHealthCareFacilitySchedule), "create", HttpMethods.Post),
            _linkService.Create(nameof(UpdateHealthCareFacilitySchedule), "update", HttpMethods.Put, new { id }),
            _linkService.Create(nameof(DeleteHealthCareFacilitySchedule), "delete", HttpMethods.Delete, new { id }),
            _linkService.Create(nameof(GetHealthCareFacilitySchedules), "all", HttpMethods.Get)
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