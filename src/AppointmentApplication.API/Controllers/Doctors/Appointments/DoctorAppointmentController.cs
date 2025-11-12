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
using AppointmentApplication.Application.Features.Appointments.Commands.CompleteAppointment;
using AppointmentApplication.Application.Features.Appointments.Commands.ConfirmAppointment;
using AppointmentApplication.Application.Features.Appointments.Commands.CreateAppointment;
using AppointmentApplication.Application.Features.Appointments.Queries.GetAppointmentByDoctorId;
using AppointmentApplication.Application.Features.Appointments.Queries.GetAppointmentDetailsForDoctorById;
using AppointmentApplication.Application.Features.Appointments.Queries.GetAppointmentForDoctorById;
using AppointmentApplication.Contracts.Requests.Appointments;
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
    [Route("api/doctors/me/appointments")]
    public class DoctorAppointmentController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;
        private readonly IUserContext _userContext;

        public DoctorAppointmentController(ISender sender, LinkService linkService, IUserContext userContext)
        {
            _sender = sender;
            _linkService = linkService;
            _userContext = userContext;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Doctor)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Get current doctor's appointments.")]
        [EndpointDescription("Retrieves paginated appointments for the currently authenticated doctor with filtering and sorting.")]
        [EndpointName("GetMyDoctorAppointments")]
        [OutputCache(Duration = 30)] // Cache for 30 seconds
        public async Task<IActionResult> GetMyDoctorAppointments(
            [FromQuery] AppointmentQueryParameters queryParameters,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                new GetAppointmentsForDoctorCommand(
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

        [HttpPost]
        [Authorize(Roles = Roles.Doctor)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Create a new appointment.")]
        [EndpointDescription("Creates a new appointment with the provided details.")]
        [EndpointName("CreateAppointmentByDoctor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAppointmentByDoctor(
            [FromBody] CreateAppointmentRequestByDoctor request,
            CancellationToken cancellationToken)
        {
            var command = new CreateAppointmentByDoctorCommand(
                _userContext.UserId,
                request.PatientId,
                request.ScheduledDate,
                request.ScheduledTime,
                request.DurationMinutes,
                request.TotalAmount);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                appointmentDto =>
                {
                    var resource = new
                    {
                        data = appointmentDto,
                    };
                    
                    // FIXED: Correct route values for CreatedAtAction
                    return CreatedAtAction(
                        nameof(GetDoctorAppointmentById), 
                        new { 
                            appointmentId = appointmentDto.Id,  // Changed from 'id' to 'appointmentId'
                            fields = (string?)null              // Added default value for fields parameter
                        }, 
                        resource);
                },
                Problem);
        }

        [HttpGet("{appointmentId:guid}")]
        [Authorize(Roles = Roles.Doctor)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Get appointment details by ID for current doctor")]
        [EndpointDescription("Retrieves detailed information about a specific appointment for the currently authenticated doctor. Returns full details including billing and prescriptions for completed appointments.")]
        [EndpointName("GetDoctorAppointmentById")]
        [OutputCache(Duration = 60, VaryByQueryKeys = new[] { "fields" })] // Cache for 60 seconds, vary by fields
        public async Task<IActionResult> GetDoctorAppointmentById(
            Guid appointmentId,
            [FromQuery] string? fields = null,
            CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(
                new GetAppointmentDetailsForDoctorByIdQuery(
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

        [HttpPut("{appointmentId:guid}/complete", Name = "CompleteDoctorAppointment")]
        [Authorize(Roles = Roles.Doctor)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Complete an appointment.")]
        [EndpointDescription("Completes an appointment, creates medical record, prescription, and marks billing as paid.")]
        [EndpointName("CompleteDoctorAppointment")]
        public async Task<IActionResult> CompleteAppointment(
            Guid appointmentId,
            [FromBody] CompleteAppointmentRequest completeRequest,
            CancellationToken cancellationToken)
        {
            var command = new CompleteAppointmentCommand(
                UserId: _userContext.UserId,
                AppointmentId: appointmentId,
                Diagnosis: completeRequest.Diagnosis,
                TreatmentNotes: completeRequest.TreatmentNotes,
                FollowUpInstructions: completeRequest.FollowUpInstructions,
                MedicationList: completeRequest.MedicationList,
                DosageInstructions: completeRequest.DosageInstructions);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                completionResult => Ok(new
                {
                    message = "Appointment completed successfully",
                    data = completionResult
                }),
                Problem);
        }

        [HttpPut("{appointmentId:guid}/confirm", Name = "ConfirmDoctorAppointment")]
        [Authorize(Roles = Roles.Doctor)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Confirm an appointment.")]
        [EndpointDescription("Confirms a pending appointment. Only the assigned doctor can confirm their own appointments.")]
        [EndpointName("ConfirmDoctorAppointment")]
        public async Task<IActionResult> ConfirmAppointment(
            Guid appointmentId,
            CancellationToken cancellationToken)
        {
            var command = new ConfirmAppointmentCommand(_userContext.UserId, appointmentId);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                Problem);
        }

        [HttpPut("{appointmentId:guid}/cancel", Name = "CancelDoctorAppointment")]
        [Authorize(Roles = $"{Roles.Doctor}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Cancel an appointment.")]
        [EndpointDescription("Cancels an appointment. Only the assigned doctor or admin can cancel appointments.")]
        [EndpointName("CancelDoctorAppointment")]
        public async Task<IActionResult> CancelAppointment(
            Guid appointmentId,
            [FromBody] CancelAppointmentRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CancelAppointmentCommand(_userContext.UserId, appointmentId, request.CancellationReason);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                Problem);
        }

        [HttpPut("{appointmentId:guid}/reschedule", Name = "RescheduleDoctorAppointment")]
        [Authorize(Roles = $"{Roles.Doctor},{Roles.Admin}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Reschedule an appointment.")]
        [EndpointDescription("Reschedules an appointment to a new date and time.")]
        [EndpointName("RescheduleDoctorAppointment")]
        public async Task<IActionResult> RescheduleAppointment(
            Guid appointmentId,
            [FromBody] RescheduleAppointmentRequest request,
            CancellationToken cancellationToken)
        {
            // You'll need to create a RescheduleAppointmentCommand
            // var command = new RescheduleAppointmentCommand(_userContext.UserId, appointmentId, request.NewDate, request.NewTime);
            // var result = await _sender.Send(command, cancellationToken);

            return Ok(new { message = "Reschedule appointment endpoint - implement RescheduleAppointmentCommand" });
        }

        private List<LinkDto> CreateLinks(string id, string? fields)
        {
            var links = new List<LinkDto>
            {
                _linkService.Create(nameof(GetMyDoctorAppointments), "my-appointments", HttpMethods.Get),
            };
            return links;
        }

        private List<LinkDto> CreateLinks(AppointmentQueryParameters parameters, bool hasNextPage, bool hasPreviousPage)
        {
            List<LinkDto> links = new()
            {
                _linkService.Create(nameof(GetMyDoctorAppointments), "self", HttpMethods.Get, new
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
                links.Add(_linkService.Create(nameof(GetMyDoctorAppointments), "next-page", HttpMethods.Get, new
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
                links.Add(_linkService.Create(nameof(GetMyDoctorAppointments), "previous-page", HttpMethods.Get, new
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
