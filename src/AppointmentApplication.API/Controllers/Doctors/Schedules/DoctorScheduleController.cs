using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Queries.GetSchedulesByIdQuery;
using AppointmentApplication.Application.HealthcareFacilities.Schedules.Queries;
using AppointmentApplication.Contracts.Requests.HealthCareFacilitites;
using AppointmentApplication.Domain.Shared.Results;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers.HealthCareFacilitites.Schedules
{
    [Route("api/health-care-facilities/{facilityId:guid}/schedules")]
    [Authorize]
    [ApiController]
    public sealed class DoctorScheduleController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;

        public DoctorScheduleController(ISender sender, LinkService linkService)
        {
            _sender = sender;
            _linkService = linkService;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointName("GetHealthCareFacilityScheduleById")]
        [EndpointSummary("Get Health Care Facility Schedule by ID")]
        [EndpointDescription("Retrieves a specific schedule for a health care facility by its unique identifier.")]
        public async Task<IActionResult> GetDoctorScheduleById(Guid facilityId, Guid id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetScheduleByIdQuery(facilityId, id), cancellationToken);

            return result.Match(
                schedule =>
                {
                    var links = CreateLinks(facilityId, id);
                    var resource = new { data = schedule, links };
                    return Ok(resource);
                },
                Problem);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [OutputCache(Duration = 60)]
        [EndpointName("GetHealthCareFacilitySchedules")]
        [EndpointSummary("Get Health Care Facility Schedules")]
        [EndpointDescription("Retrieves all available schedules for a specific health care facility. Results are cached for 60 seconds.")]
        public async Task<IActionResult> GetDoctorSchedules(Guid facilityId, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetSchedulesByIdQuery(facilityId), cancellationToken);

            return result.Match(
                schedules =>
                {
                    var resource = new { data = schedules };
                    return Ok(resource);
                },
                Problem);
        }

        private List<LinkDto> CreateLinks(Guid facilityId, Guid? scheduleId = null)
        {
            var links = new List<LinkDto>
            {
                _linkService.Create(nameof(GetDoctorSchedules), "all", HttpMethods.Get, new { facilityId }),
                _linkService.Create(nameof(GetDoctorScheduleById), "self", HttpMethods.Get, new { facilityId, id = scheduleId })
            };

            return links;
        }
    }
}