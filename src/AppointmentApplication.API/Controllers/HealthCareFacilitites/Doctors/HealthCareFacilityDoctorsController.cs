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

        // GET ALL doctors for a health care facility
        [HttpGet(Name = "get-doctors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetDoctors(
            Guid healthCareFacilityId,
            [FromQuery] DoctorQueryParameters queryParameters,
            CancellationToken cancellationToken)
        {
            // logic: fetch all doctors by healthCareFacilityId
            return Ok();
        }

        // GET doctor by ID
        [HttpGet("{id:guid}", Name = "get-doctor-by-id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        public async Task<IActionResult> GetDoctorById(
            Guid healthCareFacilityId,
            Guid id,
            CancellationToken cancellationToken)
        {
            // logic: fetch doctor by id within the healthCareFacilityId
            return Ok();
        }

        // HATEOAS links
        private List<LinkDto> CreateLinks(Guid healthCareFacilityId, Guid doctorId)
        {
            return new List<LinkDto>
            {
                _linkService.Create("get-doctor-by-id", "self", HttpMethods.Get, new { healthCareFacilityId, id = doctorId }),
                _linkService.Create("get-doctors", "all", HttpMethods.Get, new { healthCareFacilityId })
            };
        }
    }
}
