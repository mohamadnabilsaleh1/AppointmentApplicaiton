using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Features.Doctors.Schedules.Queries;

using AppointmentApplication.Contracts.Requests.Doctors;
using AppointmentApplication.Domain.Shared.Results;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers.Doctors.Schedules
{
    [Route("api/doctors/{doctorId:guid}/schedules")]
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
        [EndpointName("GetDoctorScheduleById")]
        [EndpointSummary("Get Doctor Schedule by ID")]
        [EndpointDescription("Retrieves a specific schedule for a doctor by its unique identifier.")]
        public async Task<IActionResult> GetDoctorScheduleById(Guid doctorId, Guid id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetScheduleByIdQuery(doctorId, id), cancellationToken);

            return result.Match(
                schedule =>
                {
                    var links = CreateLinks(doctorId, id);
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
        [EndpointName("GetDoctorSchedules")]
        [EndpointSummary("Get Doctor Schedules")]
        [EndpointDescription("Retrieves all available schedules for a specific doctor. Results are cached for 60 seconds.")]
        public async Task<IActionResult> GetDoctorSchedules(Guid doctorId, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetSchedulesByIdQuery(doctorId), cancellationToken);

            return result.Match(
                schedules =>
                {
                    var resource = new { data = schedules };
                    return Ok(resource);
                },
                Problem);
        }

        private List<LinkDto> CreateLinks(Guid doctorId, Guid? scheduleId = null)
        {
            var links = new List<LinkDto>
            {
                _linkService.Create(nameof(GetDoctorSchedules), "all", HttpMethods.Get, new { doctorId }),
                _linkService.Create(nameof(GetDoctorScheduleById), "self", HttpMethods.Get, new { doctorId, id = scheduleId })
            };

            return links;
        }
    }
}
