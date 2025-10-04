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

[Route("api/doctors/me/schedule-exceptions")]
[Authorize(Roles = $"{Roles.Doctor}")]
public sealed class AdminDoctorScheduleExceptionController(ISender sender, LinkService linkService) : ApiController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointName("admin-create-doctor-schedule-exception")]
    [EndpointSummary("Creates a new Doctor Schedule Exception.")]
    [EndpointDescription("Adds a new schedule exception for a doctor.")]
    public async Task<IActionResult> Create(
        [FromBody] CreateDoctorScheduleExceptionRequest request,
        CancellationToken cancellationToken)
    {
        // Security check: Doctors can only create exceptions for themselves
        return Ok();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointName("admin-get-doctor-schedule-exception-by-id")]
    [EndpointSummary("Get Doctor Schedule Exception by Id.")]
    [EndpointDescription("Retrieves a single doctor schedule exception.")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        [FromQuery] string? fields,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet]
    [MapToApiVersion("0.1")]
    [EndpointName("admin-list-doctor-schedule-exceptions")]
    [EndpointSummary("Get Doctor Schedule Exceptions.")]
    [EndpointDescription("Retrieves doctor schedule exceptions with optional filtering and pagination.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointName("admin-update-doctor-schedule-exception")]
    [EndpointSummary("Updates an existing Doctor Schedule Exception.")]
    [EndpointDescription("Updates the details of an existing doctor schedule exception.")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateDoctorScheduleExceptionRequest request,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointName("admin-delete-doctor-schedule-exception")]
    [EndpointSummary("Deletes a Doctor Schedule Exception.")]
    [EndpointDescription("Removes a doctor schedule exception from the system.")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return NoContent();
    }

    private List<LinkDto> CreateLinks(Guid id, string? fields)
    {
        var links = new List<LinkDto>
        {
            linkService.Create(nameof(GetById), "self", HttpMethods.Get, new { id, fields }),
            linkService.Create(nameof(Create), "create", HttpMethods.Post),
            linkService.Create(nameof(Update), "update", HttpMethods.Put, new { id }),
            linkService.Create(nameof(Delete), "delete", HttpMethods.Delete, new { id }),
            linkService.Create(nameof(List), "all", HttpMethods.Get)
        };
        return links;
    }
}