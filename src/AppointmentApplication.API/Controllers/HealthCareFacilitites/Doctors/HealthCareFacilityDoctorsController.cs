using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Dtos.HealthCareFacilities;
using AppointmentApplication.API.Services;
using AppointmentApplication.Contracts.Requests.Doctors;

using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers.HealthCareFacilities.Doctors
{
    [Route("api/health-care-facility/{healthCareFacilityId:guid}/doctors")]
    [Authorize]
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
        [EndpointName("GetDoctors")]
        [EndpointSummary("Retrieve all doctors for a healthcare facility")]
        [EndpointDescription("Fetches a list of all doctors associated with a specific healthcare facility by its ID.")]
        public async Task<IActionResult> GetDoctors(
            Guid healthCareFacilityId,
            [FromQuery] DoctorQueryParameters queryParameters,
            CancellationToken cancellationToken)
        {
            // logic: fetch all doctors by healthCareFacilityId
            return Ok(); // placeholder
        }

        [HttpGet("{id:guid}")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EndpointName("GetDoctorById")]
        [EndpointSummary("Retrieve a doctor by ID for a healthcare facility")]
        [EndpointDescription("Fetches a specific doctor by their ID within the context of the given healthcare facility.")]
        public async Task<IActionResult> GetDoctorById(
            Guid healthCareFacilityId,
            Guid id,
            CancellationToken cancellationToken)
        {
            // logic: fetch doctor by id within the healthCareFacilityId
            return Ok(); // placeholder
        }

        // HATEOAS links
        private List<LinkDto> CreateLinks(Guid healthCareFacilityId, Guid doctorId)
        {
            return new List<LinkDto>
            {
                _linkService.Create("GetDoctorById", "self", HttpMethods.Get, new { healthCareFacilityId, id = doctorId }),
                _linkService.Create("GetDoctors", "all", HttpMethods.Get, new { healthCareFacilityId })
            };
        }
    }
}
