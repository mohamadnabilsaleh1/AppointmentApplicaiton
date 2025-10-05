using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
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
        public async Task<IActionResult> GetDepartments(CancellationToken cancellationToken)
        {
            return Ok(); // placeholder
        }

        [HttpGet("{id:guid}")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EndpointName("GetDepartmentById")]
        [EndpointSummary("Get Department by Id.")]
        [EndpointDescription("Retrieves the details of a specific department by its Id.")]
        public async Task<IActionResult> GetDepartmentById(Guid id, string? fields, CancellationToken cancellationToken)
        {
            return Ok(); // placeholder
        }

        private List<LinkDto> CreateLinks(Guid? id = null, string? fields = null)
        {
            return new List<LinkDto>
            {
                _linkService.Create(nameof(GetDepartments), "self", HttpMethods.Get),
                _linkService.Create(nameof(GetDepartmentById), "self", HttpMethods.Get, new { id, fields })
            };
        }
    }
}