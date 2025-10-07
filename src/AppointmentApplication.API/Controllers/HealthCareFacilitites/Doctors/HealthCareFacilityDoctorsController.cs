using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Dtos.HealthCareFacilities;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Features.Doctors.Queries.GetDoctorByHealthCareFacilityIdAndUserId;
using AppointmentApplication.Application.Features.Doctors.Queries.GetDoctorsByHealthCareFacilityId;
using AppointmentApplication.Contracts.Requests.Doctors;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers.HealthCareFacilities.Doctors
{
    [Route("api/health-care-facilities/{healthCareFacilityId:guid}/doctors")]
    public sealed class HealthCareFacilityDoctorsController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;

        public HealthCareFacilityDoctorsController(ISender sender, LinkService linkService)
        {
            _sender = sender;
            _linkService = linkService;
        }

        [HttpGet]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [OutputCache(Duration = 60)]
        [EndpointName("GetDoctorsByHealthCareFacilityId")]
        [EndpointSummary("Retrieve all doctors for a healthcare facility")]
        [EndpointDescription("Fetches a list of all doctors associated with a specific healthcare facility by its ID.")]
        public async Task<IActionResult> GetDoctorsByHealthCareFacilityId( // Renamed method to match endpoint name
            Guid healthCareFacilityId,
            [FromQuery] DoctorQueryParameters queryParameters,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetDoctorsByHealthCareFacilityIdQuery(healthCareFacilityId), cancellationToken);
            return result.Match(
                doctors =>
                {
                    var resource = new { data = doctors };
                    return Ok(resource);
                },
                Problem);
        }

        [HttpGet("{id:guid}")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EndpointName("GetDoctorByIdAndByHealthCareFacilityId")]
        [EndpointSummary("Retrieve a doctor by ID for a healthcare facility")]
        [EndpointDescription("Fetches a specific doctor by their ID within the context of the given healthcare facility.")]
        public async Task<IActionResult> GetDoctorByIdAndByHealthCareFacilityId(
            Guid healthCareFacilityId,
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetDoctorByHealthCareFacilityIdAndDoctorIdQuery(healthCareFacilityId, id), cancellationToken);

            return result.Match(
                doctor =>
                {
                    var links = CreateLinks(healthCareFacilityId, id);
                    var resource = new { data = doctor, links };
                    return Ok(resource);
                },
                Problem);
        }

        // HATEOAS links - CORRECTED
        private List<LinkDto> CreateLinks(Guid healthCareFacilityId, Guid? doctorId)
        {
            var links = new List<LinkDto>
            {
                _linkService.Create(
                    "GetDoctorsByHealthCareFacilityId", 
                    "all", 
                    HttpMethods.Get, 
                    new { healthCareFacilityId }),
                
                _linkService.Create(
                    "GetDoctorByIdAndByHealthCareFacilityId", 
                    "self", 
                    HttpMethods.Get, 
                    new { healthCareFacilityId, id = doctorId })
            };

            return links;
        }
    }
}