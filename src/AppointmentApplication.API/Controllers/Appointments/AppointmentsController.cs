using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Api.Models.Appointments;
using AppointmentApplication.API;
using AppointmentApplication.API.Controllers;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Appointments.Commands.CreateAppointment;
using AppointmentApplication.Application.Features.Doctors.Queries.GetDoctorByHealthCareFacilityIdAndUserId;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Roles.Patient}")]
    public class AppointmentsController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;
        private readonly IUserContext _userContext;

        public AppointmentsController(ISender sender, LinkService linkService, IUserContext userContext)
        {
            _sender = sender;
            _linkService = linkService;
            _userContext = userContext;
        }

        [HttpPost]
        [Authorize]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Create a new appointment.")]
        [EndpointDescription("Creates a new appointment with the provided details.")]
        [EndpointName("CreateAppointment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAppointment(
    [FromBody] CreateAppointmentRequest request,
    CancellationToken cancellationToken)
        {
            var command = new CreateAppointmentCommand(
                _userContext.UserId,
                request.DoctorId,
                request.FacilityId,
                request.ScheduledDate,
                request.ScheduledTime,
                request.DurationMinutes,
                request.Notes,
                request.TotalAmount);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                appointment =>
                {
                    var resource = new
                    {
                        data = appointment,
                    };
                    return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment }, resource);
                },
                Problem);
        }
        [HttpGet("{id:guid}", Name = "GetAppointmentById")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Get Appointment by Id.")]
        [EndpointDescription("Retrieves a single appointment.")]
        [EndpointName("GetAppointmentById")]
        public async Task<IActionResult> GetAppointmentById(
    Guid id,
    string? fields,
    CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}