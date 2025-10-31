using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Queries.GetDoctorTreatmentCapabilityByDoctorId;
using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Queries.GetDoctorTreatmentCapabilityById;
using AppointmentApplication.Domain.Shared.Results;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers.Doctors.DoctorsTreatmentCapabilities
{
    [Route("api/doctors/{doctorId:guid}/treatment-capacity")]
    [Authorize(Roles = $"{Roles.Doctor}")]
    public class DoctorsTreatmentCapacityController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;

        public DoctorsTreatmentCapacityController(ISender sender, LinkService linkService)
        {
            _sender = sender;
            _linkService = linkService;
        }

        [HttpGet]
        [MapToApiVersion("0.1")]
        [EndpointName("get-doctor-treatment-capacity-by-id")]
        [EndpointSummary("Get Doctor Treatment Capacity by Doctor ID.")]
        [EndpointDescription("Retrieves the treatment capacity configuration for a specific doctor.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetTreatmentCapacityByDoctorId(
            [FromRoute] Guid doctorId,
            CancellationToken cancellationToken)
        {
            var query = new GetDoctorTreatmentCapabilityByDoctorIdQuery(doctorId);
            var result = await _sender.Send(query, cancellationToken);

            return result.Match(
                treatmentCapacity =>
                {
                    var links = CreateLinks(doctorId);
                    var resource = new { data = treatmentCapacity, links };
                    return Ok(resource);
                },
                Problem);
        }

        private List<LinkDto> CreateLinks(Guid doctorId)
        {
            var links = new List<LinkDto>
            {
                _linkService.Create(nameof(GetTreatmentCapacityByDoctorId), "self", HttpMethods.Get, new { doctorId })
            };
            return links;
        }
    }
}