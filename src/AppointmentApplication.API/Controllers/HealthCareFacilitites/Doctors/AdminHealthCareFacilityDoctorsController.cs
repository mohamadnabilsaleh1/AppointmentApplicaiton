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

        // GET ALL
        [HttpGet(Name = "admin-get-doctors")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetDoctors(DoctorQueryParameters doctorQueryParameters,CancellationToken cancellationToken)
        {
            // TODO: implement fetching all doctors logic
            return Ok(new List<object>()); // return list of doctors
        }

        // GET BY ID
        [HttpGet("{id:guid}", Name = "admin-get-doctor-by-id")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDoctorById(Guid id, CancellationToken cancellationToken)
        {
            // TODO: implement fetching doctor by id
            return Ok(new { Id = id, Name = "Doctor Name" });
        }

        // CREATE
        [HttpPost(Name = "admin-create-doctor")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorRequest request, CancellationToken cancellationToken)
        {
            // TODO: implement doctor creation logic
            var createdDoctorId = Guid.NewGuid(); // placeholder
            return CreatedAtRoute("get-doctor-by-id", new { id = createdDoctorId }, request);
        }

        // UPDATE
        [HttpPut("{id:guid}", Name = "admin-update-doctor")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDoctor(Guid id, [FromBody] UpdateDoctorRequest request, CancellationToken cancellationToken)
        {
            // TODO: implement doctor update logic
            return Ok(request);
        }

        // DELETE
        [HttpDelete("{id:guid}", Name = "admin-delete-doctor")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDoctor(Guid id, CancellationToken cancellationToken)
        {
            // TODO: implement doctor deletion logic
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
