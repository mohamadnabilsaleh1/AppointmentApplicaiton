using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Queries.GetDepartmentById;
using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Queries.GetDepartmentsById;
using AppointmentApplication.Contracts.Requests.Departments;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers
{
    [Route("api/health-care-facilities/{facilityId:guid}/departments")]
    [Authorize]
    public sealed class HealthCareFacilityDepartmentController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;

        public HealthCareFacilityDepartmentController(ISender sender, LinkService linkService)
        {
            _sender = sender;
            _linkService = linkService;
        }

        [HttpGet]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [OutputCache(Duration = 60)]
        [EndpointName("GetDepartments")]
        [EndpointSummary("Get all Departments for the Facility.")]
        [EndpointDescription("Retrieves a list of all departments belonging to the specified healthcare facility.")]
        public async Task<IActionResult> GetDepartments(Guid facilityId, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetDepartmentsByIdQuery(facilityId), cancellationToken);

            return result.Match(
                schedule =>
                {
                    var resource = new { data = schedule };
                    return Ok(resource);
                },
                Problem);
        }

        [HttpGet("{id:guid}")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EndpointName("GetDepartmentById")]
        [EndpointSummary("Get Department by Id.")]
        [EndpointDescription("Retrieves the details of a specific department by its Id.")]
        public async Task<IActionResult> GetDepartmentById(Guid id, Guid facilityId, string? fields, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetDepartmentByIdQuery(facilityId, id), cancellationToken);

            return result.Match(
                schedule =>
                {
                    var links = CreateLinks(facilityId, id);
                    var resource = new { data = schedule, links };
                    return Ok(resource);
                },
                Problem);
        }

        private List<LinkDto> CreateLinks(Guid facilityId, Guid? scheduleId = null)
        {
            var links = new List<LinkDto>
            {
                _linkService.Create(nameof(GetDepartments), "all", HttpMethods.Get, new { facilityId }),
                _linkService.Create(nameof(GetDepartmentById), "self", HttpMethods.Get, new { facilityId, id = scheduleId })
            };

            return links;
        }
    }
}