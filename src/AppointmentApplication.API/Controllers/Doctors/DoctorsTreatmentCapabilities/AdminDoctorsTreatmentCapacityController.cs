using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Commands.CreateDoctorsTreatmentCapability;
using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Commands.UpdateDoctorsTreatmentCapability;
using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Queries.GetDoctorTreatmentCapabilityById;
using AppointmentApplication.Contracts.Requests.Doctors.DoctorsTreatmentCapabilities;
using AppointmentApplication.Domain.Shared.Results;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers.Doctors.DoctorsTreatmentCapabilities
{
    [Route("api/doctors/me/treatment-capacity")]
    [Authorize(Roles = $"{Roles.Doctor}")]
    public sealed class AdminDoctorsTreatmentCapacityController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;
        private readonly IUserContext _userContext;

        public AdminDoctorsTreatmentCapacityController(ISender sender, LinkService linkService, IUserContext userContext)
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
        [EndpointName("create-doctor-treatment-capacity")]
        [EndpointSummary("Creates a new Doctor Treatment Capacity.")]
        [EndpointDescription("Defines how many patients a doctor can treat per day and the session duration.")]
        public async Task<IActionResult> AdminCreate(
            [FromBody] CreateDoctorsTreatmentCapacityRequest request,
            CancellationToken cancellationToken)
        {
            // Extract doctor ID from authenticated user (assuming you have a way to get current user ID)
            var doctorId = _userContext.UserId;
            var command = new CreateDoctorsTreatmentCapabilityCommand(
                doctorId,
                request.MaxPatientsPerDay,
                request.SessionDurationMinutes);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                treatmentCapacity =>
                {
                    var links = CreateLinks(doctorId, null);
                    var resource = new { data = treatmentCapacity, links };
                    return CreatedAtAction(nameof(AdminGetTreatmentCapacity), new { doctorId }, resource);
                },
                Problem);
        }

        [HttpGet]
        [MapToApiVersion("0.1")]
        [EndpointName("get-doctor-treatment-capacity")]
        [EndpointSummary("Get Doctor Treatment Capacity.")]
        [EndpointDescription("Retrieves the current doctor's treatment capacity configuration.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> AdminGetTreatmentCapacity(CancellationToken cancellationToken)
        {
            var doctorId = _userContext.UserId;

            var query = new GetDoctorTreatmentCapabilityByIdQuery(doctorId);
            var result = await _sender.Send(query, cancellationToken);

            return result.Match(
                treatmentCapacity =>
                {
                    var links = CreateLinks(doctorId, null);
                    var resource = new { data = treatmentCapacity, links };
                    return Ok(resource);
                },
                Problem);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointName("update-doctor-treatment-capacity")]
        [EndpointSummary("Updates the current Doctor Treatment Capacity.")]
        [EndpointDescription("Updates the max patients per day and session duration for the current doctor.")]
        public async Task<IActionResult> AdminUpdate(
            [FromBody] UpdateDoctorsTreatmentCapacityRequest request,
            CancellationToken cancellationToken)
        {
            var doctorId = _userContext.UserId;
            var command = new UpdateDoctorsTreatmentCapabilityCommand(
                doctorId,
                request.MaxPatientsPerDay,
                request.SessionDurationMinutes);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(_ => NoContent(), Problem);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointName("delete-doctor-treatment-capacity")]
        [EndpointSummary("Deletes the current Doctor Treatment Capacity.")]
        [EndpointDescription("Removes the current doctor's treatment capacity configuration.")]
        public async Task<IActionResult> AdminDelete(CancellationToken cancellationToken)
        {
            var command = new DeleteDoctorsTreatmentCapabilityCommand(_userContext.UserId);
            var result = await _sender.Send(command, cancellationToken);
            return result.Match<IActionResult>(
                _ => NoContent(),
                Problem);
        }

        private List<LinkDto> CreateLinks(Guid doctorId, string? fields)
        {
            var links = new List<LinkDto>
            {
                _linkService.Create(nameof(AdminCreate), "create", HttpMethods.Post),
                _linkService.Create(nameof(AdminGetTreatmentCapacity), "self", HttpMethods.Get),
                _linkService.Create(nameof(AdminUpdate), "update", HttpMethods.Put),
                _linkService.Create(nameof(AdminDelete), "delete", HttpMethods.Delete)
            };
            return links;
        }
    }
}