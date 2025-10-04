using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Dtos.HealthCareFacilities;
using AppointmentApplication.API.Services;
using AppointmentApplication.Contracts.Requests.Doctors;
using AppointmentApplication.Domain.Users;

using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers.HealthCareFacilitites.Doctors
{
    [Route("api/health-care-facility/me/doctors")]
    [Authorize(Roles = Roles.HealthCareFacility)]
    public sealed class AdminHealthCareFacilityDoctorsController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;

        public AdminHealthCareFacilityDoctorsController(ISender sender, LinkService linkService)
        {
            _sender = sender;
            _linkService = linkService;
        }

        [HttpGet]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [OutputCache(Duration = 60)]
        [EndpointName("AdminGetDoctors")]
        [EndpointSummary("Retrieve all doctors")]
        [EndpointDescription("Fetches all doctors for the currently authenticated health care facility.")]
        public async Task<IActionResult> GetDoctors(DoctorQueryParameters doctorQueryParameters, CancellationToken cancellationToken)
        {
            return Ok(new List<object>()); // placeholder
        }

        [HttpGet("{id:guid}")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EndpointName("AdminGetDoctorById")]
        [EndpointSummary("Retrieve doctor by ID")]
        [EndpointDescription("Fetches a specific doctor for the currently authenticated health care facility by ID.")]
        public async Task<IActionResult> GetDoctorById(Guid id, CancellationToken cancellationToken)
        {
            return Ok(new { Id = id, Name = "Doctor Name" }); // placeholder
        }

        [HttpPost]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EndpointName("AdminCreateDoctor")]
        [EndpointSummary("Create a new doctor")]
        [EndpointDescription("Adds a new doctor to the currently authenticated health care facility.")]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorRequest request, CancellationToken cancellationToken)
        {
            var createdDoctorId = Guid.NewGuid(); // placeholder
            return CreatedAtRoute("GetDoctorById", new { id = createdDoctorId }, request);
        }

        [HttpPut("{id:guid}")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EndpointName("AdminUpdateDoctor")]
        [EndpointSummary("Update a doctor")]
        [EndpointDescription("Modifies an existing doctor in the currently authenticated health care facility.")]
        public async Task<IActionResult> UpdateDoctor(Guid id, [FromBody] UpdateDoctorRequest request, CancellationToken cancellationToken)
        {
            return Ok(request);
        }

        [HttpDelete("{id:guid}")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EndpointName("AdminDeleteDoctor")]
        [EndpointSummary("Delete a doctor")]
        [EndpointDescription("Removes a specific doctor from the currently authenticated health care facility.")]
        public async Task<IActionResult> DeleteDoctor(Guid id, CancellationToken cancellationToken)
        {
            return NoContent();
        }

        private List<LinkDto> CreateLinks(Guid id)
        {
            return new List<LinkDto>
            {
                _linkService.Create(nameof(GetDoctorById), "self", HttpMethods.Get, new { id }),
                _linkService.Create(nameof(CreateDoctor), "create", HttpMethods.Post),
                _linkService.Create(nameof(UpdateDoctor), "update", HttpMethods.Put, new { id }),
                _linkService.Create(nameof(DeleteDoctor), "delete", HttpMethods.Delete, new { id }),
                _linkService.Create(nameof(GetDoctors), "all", HttpMethods.Get)
            };
        }
    }
}
