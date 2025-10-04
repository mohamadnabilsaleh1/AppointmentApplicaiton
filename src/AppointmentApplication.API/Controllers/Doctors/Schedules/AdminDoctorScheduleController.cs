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

[Route("api/doctors/me/schedules")]
[Authorize(Roles = $"{Roles.Doctor}")]

public sealed class AdminDoctorScheduleController(ISender sender, LinkService linkService) : ApiController
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Creates a new Doctor Schedule.")]
    [EndpointDescription("Adds a new schedule for a doctor.")]
    [EndpointName("CreateDoctorSchedule")]
    public async Task<IActionResult> CreateDoctorSchedule(
        Guid doctorId,
        [FromBody] CreateDoctorScheduleRequest request,
        CancellationToken cancellationToken)
    {

        return Ok();
    }

    [HttpGet("{id:guid}", Name = "GetDoctorScheduleById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Doctor Schedule by Id.")]
    [EndpointDescription("Retrieves a single doctor schedule.")]
    [EndpointName("GetDoctorScheduleById")]
    public async Task<IActionResult> GetDoctorScheduleById(
        Guid doctorId,
        Guid id,
        string? fields,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Doctor Schedules.")]
    [EndpointDescription("Retrieves doctor schedules with optional filtering and pagination.")]
    [EndpointName("GetDoctorSchedules")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> GetDoctorSchedules(
        Guid doctorId,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Updates an existing Doctor Schedule.")]
    [EndpointDescription("Updates the details of an existing doctor schedule.")]
    [EndpointName("UpdateDoctorSchedule")]
    public async Task<IActionResult> UpdateDoctorSchedule(
        Guid doctorId,
        Guid id,
        [FromBody] UpdateDoctorScheduleRequest request,
        CancellationToken cancellationToken)
    {
        // Security check: Doctors can only update their own schedules
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Deletes a Doctor Schedule.")]
    [EndpointDescription("Removes a doctor schedule from the system.")]
    [EndpointName("DeleteDoctorSchedule")]
    public async Task<IActionResult> DeleteDoctorSchedule(
        Guid doctorId,
        Guid id,
        CancellationToken cancellationToken)
    {

        return NoContent();
    }

    private List<LinkDto> CreateLinks(Guid doctorId, string id, string? fields)
    {
        var links = new List<LinkDto>
        {
            linkService.Create(nameof(GetDoctorScheduleById), "self", HttpMethods.Get, new { doctorId, id, fields }),
            linkService.Create(nameof(CreateDoctorSchedule), "create", HttpMethods.Post, new { doctorId }),
            linkService.Create(nameof(UpdateDoctorSchedule), "update", HttpMethods.Put, new { doctorId, id }),
            linkService.Create(nameof(DeleteDoctorSchedule), "delete", HttpMethods.Delete, new { doctorId, id }),
            linkService.Create(nameof(GetDoctorSchedules), "all", HttpMethods.Get, new { doctorId })
        };


        return links;
    }


}