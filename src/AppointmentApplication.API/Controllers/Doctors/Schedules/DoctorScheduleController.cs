using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Contracts.Requests.Doctors.Schedules;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/doctors/{doctor-id:guid}/schedules")]
[Authorize(Roles = $"{Roles.Doctor}")]
public sealed class DoctorScheduleController(ISender sender, LinkService linkService) : ApiController
{
    [HttpGet("{schedule-id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointName("get-schedule")]
    [EndpointSummary("Get Doctor Schedule by Id.")]
    [EndpointDescription("Retrieves a single doctor schedule.")]
    public async Task<IActionResult> GetSchedule(
        [FromRoute(Name = "doctor-id")] Guid doctorId,
        [FromRoute(Name = "schedule-id")] Guid scheduleId,
        [FromQuery] string? fields,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet]
    [MapToApiVersion("0.1")]
    [EndpointName("list-schedules")]
    [EndpointSummary("Get Doctor Schedules.")]
    [EndpointDescription("Retrieves doctor schedules with optional filtering and pagination.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> GetSchedules(
        [FromRoute(Name = "doctor-id")] Guid doctorId,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    private List<LinkDto> CreateLinks(Guid doctorId, Guid scheduleId, string? fields)
    {
        var links = new List<LinkDto>
        {
            linkService.Create(nameof(GetSchedule), "self", HttpMethods.Get, new { doctorId, scheduleId, fields }),
            linkService.Create(nameof(GetSchedules), "all", HttpMethods.Get, new { doctorId })
        };

        return links;
    }
}