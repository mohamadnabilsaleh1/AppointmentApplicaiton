using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Contracts.Requests.HealthCareFacilitites;
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
    public sealed class HealthCareFacilityScheduleController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;

        public HealthCareFacilityScheduleController(ISender sender, LinkService linkService)
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
        public async Task<IActionResult> GetHealthCareFacilityScheduleById(Guid facilityId, Guid id, CancellationToken cancellationToken)
        {
            // var result = await _sender.Send(new GetHealthCareFacilityScheduleByIdQuery(facilityId, id), cancellationToken);

            // return result.Match(
            //     schedule => {
            //         var links = CreateLinks(facilityId, id);
            //         var resource = new { data = schedule, links };
            //         return Ok(resource);
            //     },
            //     Problem);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [OutputCache(Duration = 60)]
        [EndpointName("GetHealthCareFacilitySchedules")]
        [EndpointSummary("Get Health Care Facility Schedules")]
        [EndpointDescription("Retrieves all available schedules for a specific health care facility. Results are cached for 60 seconds.")]
        public async Task<IActionResult> GetHealthCareFacilitySchedules(Guid facilityId, CancellationToken cancellationToken)
        {
            // var result = await _sender.Send(new GetHealthCareFacilitySchedulesQuery(facilityId), cancellationToken);

            // return result.Match(
            //     schedules => {
            //         var links = CreateLinks(facilityId);
            //         var resource = new { data = schedules, links };
            //         return Ok(resource);
            //     },
            //     Problem);
            return Ok();
        }

        private List<LinkDto> CreateLinks(Guid facilityId, Guid? scheduleId = null)
        {
            var links = new List<LinkDto>
            {
                _linkService.Create(nameof(GetHealthCareFacilitySchedules), "all", HttpMethods.Get, new { facilityId }),
                _linkService.Create(nameof(GetHealthCareFacilityScheduleById), "self", HttpMethods.Get, new { facilityId, id = scheduleId })
            };

            return links;
        }
    }
}