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

[Route("api/doctors/{doctorId}/schedule-exceptions")]
[Authorize(Roles = $"{Roles.Admin}, {Roles.Doctor}, {Roles.HealthCareFacility}")]
public sealed class DoctorScheduleExceptionController(ISender sender, LinkService linkService) : ApiController
{
    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Doctor}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Creates a new Doctor Schedule Exception.")]
    [EndpointDescription("Adds a new schedule exception for a doctor.")]
    [EndpointName("CreateDoctorScheduleException")]
    public async Task<IActionResult> CreateDoctorScheduleException(
        Guid doctorId,
        [FromBody] CreateDoctorScheduleExceptionRequest request,
        CancellationToken cancellationToken)
    {
        // Security check: Doctors can only create exceptions for themselves

        return Ok();
    }

    [HttpGet("{id:guid}", Name = "GetDoctorScheduleExceptionById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Doctor Schedule Exception by Id.")]
    [EndpointDescription("Retrieves a single doctor schedule exception.")]
    [EndpointName("GetDoctorScheduleExceptionById")]
    public async Task<IActionResult> GetDoctorScheduleExceptionById(
        Guid doctorId,
        Guid id,
        string? fields,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Doctor Schedule Exceptions.")]
    [EndpointDescription("Retrieves doctor schedule exceptions with optional filtering and pagination.")]
    [EndpointName("GetDoctorScheduleExceptions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> GetDoctorScheduleExceptions(
        Guid doctorId,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Doctor}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Updates an existing Doctor Schedule Exception.")]
    [EndpointDescription("Updates the details of an existing doctor schedule exception.")]
    [EndpointName("UpdateDoctorScheduleException")]
    public async Task<IActionResult> UpdateDoctorScheduleException(
        Guid doctorId,
        Guid id,
        [FromBody] UpdateDoctorScheduleExceptionRequest request,
        CancellationToken cancellationToken)
    {

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Doctor}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Deletes a Doctor Schedule Exception.")]
    [EndpointDescription("Removes a doctor schedule exception from the system.")]
    [EndpointName("DeleteDoctorScheduleException")]
    public async Task<IActionResult> DeleteDoctorScheduleException(
        Guid doctorId,
        Guid id,
        CancellationToken cancellationToken)
    {

        return NoContent();
    }

    [HttpGet("me", Name = "GetMyDoctorScheduleExceptions")]
    [Authorize(Roles = Roles.Doctor)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get current doctor's schedule exceptions.")]
    [EndpointDescription("Retrieves schedule exceptions for the currently authenticated doctor.")]
    [EndpointName("GetMyDoctorScheduleExceptions")]
    public async Task<IActionResult> GetMyDoctorScheduleExceptions(
        CancellationToken cancellationToken)
    {

        return Ok();
    }

    private List<LinkDto> CreateLinks(Guid doctorId, string id, string? fields)
    {
        var links = new List<LinkDto>
        {
            linkService.Create(nameof(GetDoctorScheduleExceptionById), "self", HttpMethods.Get, new { doctorId, id, fields }),
            linkService.Create(nameof(CreateDoctorScheduleException), "create", HttpMethods.Post, new { doctorId }),
            linkService.Create(nameof(UpdateDoctorScheduleException), "update", HttpMethods.Put, new { doctorId, id }),
            linkService.Create(nameof(DeleteDoctorScheduleException), "delete", HttpMethods.Delete, new { doctorId, id }),
            linkService.Create(nameof(GetDoctorScheduleExceptions), "all", HttpMethods.Get, new { doctorId })
        };
        return links;
    }

}