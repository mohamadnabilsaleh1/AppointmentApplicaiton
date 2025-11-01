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
using AppointmentApplication.Application.Features.Appointments.Commands.CreateAppointment;
using AppointmentApplication.Application.Features.Appointments.Queries.GetAppointmentByDoctorId;
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