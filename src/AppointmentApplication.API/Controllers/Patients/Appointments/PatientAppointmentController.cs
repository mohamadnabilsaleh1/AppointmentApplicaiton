using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Api.Models.Appointments;
using AppointmentApplication.API;
using AppointmentApplication.API.Controllers;
using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Models.Appointments;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Appointments.Commands.CancelAppointment;
using AppointmentApplication.Application.Features.Appointments.Commands.CreateAppointment;
using AppointmentApplication.Application.Features.Appointments.Commands.RescheduleAppointment;
using AppointmentApplication.Application.Features.Appointments.Queries.GetAppointmentByDoctorId;
using AppointmentApplication.Application.Features.Appointments.Queries.GetAppointmentDetailsForPatientById;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.Api.Controllers
{
    [Route("api/patients/me/appointments")]
    public class PatientAppointmentController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;
        private readonly IUserContext _userContext;

        public PatientAppointmentController(ISender sender, LinkService linkService, IUserContext userContext)
        {
            _sender = sender;
            _linkService = linkService;
            _userContext = userContext;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Patient)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Get current doctor's appointments.")]
        [EndpointDescription("Retrieves paginated appointments for the currently authenticated doctor with filtering and sorting.")]
        [EndpointName("GetMyPatientAppointments")]
        [OutputCache(Duration = 30)] // Cache for 30 seconds
        public async Task<IActionResult> GetMyPatientAppointments(
            [FromQuery] AppointmentQueryParameters queryParameters,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                new GetAppointmentsForPatientCommand(
                    UserId: _userContext.UserId,
                    StartDate: queryParameters.StartDate,
                    EndDate: queryParameters.EndDate,
                    Status: queryParameters.Status,
                    Search: queryParameters.Search,
                    Sort: queryParameters.Sort,
                    Page: queryParameters.Page,
                    PageSize: queryParameters.PageSize,
                    Fields: queryParameters.Fields),
                cancellationToken);

            return result.Match(
                response =>
                {

                    var hasNextPage = response.Page < response.TotalPages;
                    var hasPreviousPage = response.Page > 1;

                    var links = CreateLinks(queryParameters, hasNextPage, hasPreviousPage);

                    var resource = new
                    {
                        data = response.Items,
                        pagination = new
                        {
                            response.Page,
                            response.PageSize,
                            response.TotalCount,
                            response.TotalPages
                        },
                        links
                    };

                    return Ok(resource);
                },
                Problem);
        }
        [HttpGet("{appointmentId:guid}")]
        [Authorize(Roles = Roles.Patient)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Get appointment details by ID for current patient")]
        [EndpointDescription("Retrieves detailed information about a specific appointment for the currently authenticated patient. Returns full details including billing and prescriptions for completed appointments.")]
        [EndpointName("GetPatientAppointmentById")]
        [OutputCache(Duration = 60, VaryByQueryKeys = new[] { "fields" })] // Cache for 60 seconds, vary by fields
        public async Task<IActionResult> GetPatientAppointmentById(
    Guid appointmentId,
    [FromQuery] string? fields = null,
    CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(
                new GetAppointmentDetailsForPatientByIdQuery(
                    UserId: _userContext.UserId,
                    AppointmentId: appointmentId,
                    Fields: fields),
                cancellationToken);

            return result.Match(
                appointmentDetails =>
                {
                    var resource = new
                    {
                        data = appointmentDetails
                    };

                    return Ok(resource);
                },
                Problem);
        }

        [HttpPut("{appointmentId:guid}/cancel", Name = "CancelPatientAppointment")]
        [Authorize(Roles = Roles.Patient)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Cancel an appointment (patient).")]
        [EndpointDescription("Cancels an appointment for the authenticated patient. Must be more than 24 hours before the scheduled time.")]
        [EndpointName("CancelPatientAppointment")]
        public async Task<IActionResult> CancelAppointment(
            Guid appointmentId,
            [FromBody] CancelAppointmentRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CancelAppointmentByPatientIdCommand(
                _userContext.UserId,
                appointmentId,
                request.CancellationReason);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                Problem);
        }

        [HttpPut("{appointmentId:guid}/reschedule", Name = "ReschedulePatientAppointment")]
        [Authorize(Roles = Roles.Patient)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Reschedule an appointment (patient).")]
        [EndpointDescription("Reschedules an appointment for the authenticated patient. Must be more than 24 hours before the scheduled time.")]
        [EndpointName("ReschedulePatientAppointment")]
        public async Task<IActionResult> RescheduleAppointment(
            Guid appointmentId,
            [FromBody] RescheduleAppointmentRequest request,
            CancellationToken cancellationToken)
        {
            var command = new RescheduleAppointmentByPatientIdCommand(
                _userContext.UserId,
                appointmentId,
                request.NewDate,
                request.NewTime);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => Ok(new
                {
                    message = "Appointment rescheduled successfully",
                    appointmentId,
                    request.NewDate,
                    request.NewTime
                }),
                Problem);
        }

        private List<LinkDto> CreateLinks(string id, string? fields)
        {
            var links = new List<LinkDto>
            {
                _linkService.Create(nameof(GetMyPatientAppointments), "my-appointments", HttpMethods.Get),
            };
            return links;
        }

        private List<LinkDto> CreateLinks(AppointmentQueryParameters parameters, bool hasNextPage, bool hasPreviousPage)
        {
            List<LinkDto> links = new()
            {
                _linkService.Create(nameof(GetMyPatientAppointments), "self", HttpMethods.Get, new
                {
                    page = parameters.Page,
                    pageSize = parameters.PageSize,
                    fields = parameters.Fields,
                    search = parameters.Search,
                    sort = parameters.Sort,
                    startDate = parameters.StartDate,
                    endDate = parameters.EndDate,
                    status = parameters.Status
                }),
            };

            if (hasNextPage)
            {
                links.Add(_linkService.Create(nameof(GetMyPatientAppointments), "next-page", HttpMethods.Get, new
                {
                    page = parameters.Page + 1,
                    pageSize = parameters.PageSize,
                    fields = parameters.Fields,
                    search = parameters.Search,
                    sort = parameters.Sort,
                    startDate = parameters.StartDate,
                    endDate = parameters.EndDate,
                    status = parameters.Status
                }));
            }

            if (hasPreviousPage)
            {
                links.Add(_linkService.Create(nameof(GetMyPatientAppointments), "previous-page", HttpMethods.Get, new
                {
                    page = parameters.Page - 1,
                    pageSize = parameters.PageSize,
                    fields = parameters.Fields,
                    search = parameters.Search,
                    sort = parameters.Sort,
                    startDate = parameters.StartDate,
                    endDate = parameters.EndDate,
                    status = parameters.Status
                }));
            }

            return links;
        }
    }
}
