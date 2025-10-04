using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Contracts.Requests.Doctors.DoctorsTreatmentCapabilities;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers.Doctors.DoctorsTreatmentCapabilities
{
    [Route("api/doctors/me/treatment-capacity")]
    [Authorize(Roles = $"{Roles.Doctor}")]
    public sealed class DoctorsTreatmentCapacityController(ISender sender, LinkService linkService) : ApiController
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointName("create-doctor-treatment-capacity")]
        [EndpointSummary("Creates a new Doctor Treatment Capacity.")]
        [EndpointDescription("Defines how many patients a doctor can treat per day and the session duration.")]
        public async Task<IActionResult> Create(
            [FromBody] CreateDoctorsTreatmentCapacityRequest request,
            CancellationToken cancellationToken)
        {
            // TODO: implement logic
            return Ok();
        }


        [HttpGet]
        [MapToApiVersion("0.1")]
        [EndpointName("list-doctor-treatment-capacities")]
        [EndpointSummary("Get Doctor Treatment Capacities.")]
        [EndpointDescription("Retrieves all doctor treatment capacities with optional filtering and pagination.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetTreatmentCapacity(CancellationToken cancellationToken)
        {
            // TODO: implement logic
            return Ok();
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointName("update-doctor-treatment-capacity")]
        [EndpointSummary("Updates an existing Doctor Treatment Capacity.")]
        [EndpointDescription("Updates the max patients per day, session duration, and status.")]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateDoctorsTreatmentCapacityRequest request,
            CancellationToken cancellationToken)
        {
            // TODO: implement logic
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointName("delete-doctor-treatment-capacity")]
        [EndpointSummary("Deletes a Doctor Treatment Capacity.")]
        [EndpointDescription("Removes a doctor's treatment capacity configuration.")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            // TODO: implement logic
            return NoContent();
        }

        private List<LinkDto> CreateLinks(Guid id, string? fields)
        {
            var links = new List<LinkDto>
            {
                linkService.Create(nameof(Create), "create", HttpMethods.Post),
                linkService.Create(nameof(Update), "update", HttpMethods.Put, new { id }),
                linkService.Create(nameof(Delete), "delete", HttpMethods.Delete, new { id }),
                linkService.Create(nameof(GetTreatmentCapacity), "self", HttpMethods.Get)
            };
            return links;
        }
    }
}
