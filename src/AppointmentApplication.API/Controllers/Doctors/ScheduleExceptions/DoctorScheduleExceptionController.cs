using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Contracts.Requests.Doctors.ScheduleExceptions;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/doctors/{doctor-id:guid}/schedule-exceptions")]
[Authorize(Roles = $"{Roles.Admin}, {Roles.Doctor}, {Roles.HealthCareFacility}")]
public sealed class DoctorScheduleExceptionController(ISender sender, LinkService linkService) : ApiController
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointName("get-doctor-schedule-exception-by-id")]
    [EndpointSummary("Get Doctor Schedule Exception by Id.")]
    [EndpointDescription("Retrieves a single doctor schedule exception.")]
    public async Task<IActionResult> GetById(
        [FromRoute(Name = "doctor-id")] Guid doctorId,
        [FromRoute] Guid id,
        [FromQuery] string? fields,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet]
    [MapToApiVersion("0.1")]
    [EndpointName("list-doctor-schedule-exceptions")]
    [EndpointSummary("Get Doctor Schedule Exceptions.")]
    [EndpointDescription("Retrieves doctor schedule exceptions with optional filtering and pagination.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> GetScheduleExceptions(
        [FromRoute(Name = "doctor-id")] Guid doctorId,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    private List<LinkDto> CreateLinks(Guid doctorId, Guid scheduleExceptionId, string? fields)
    {
        var links = new List<LinkDto>
        {
            linkService.Create(nameof(GetById), "self", HttpMethods.Get, new { doctorId, scheduleExceptionId, fields }),
            linkService.Create(nameof(GetScheduleExceptions), "all", HttpMethods.Get, new { doctorId }),
        };
        return links;
    }
}