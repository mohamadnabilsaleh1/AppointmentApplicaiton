
using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Contracts.Requests.Departments;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/health-care-facility/me/departments")]
[Authorize(Roles = $"{Roles.HealthCareFacility}")]
public sealed class AdminHealthCareFacilityDepartmentController(ISender sender, LinkService linkService) : ApiController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Creates a new Department.")]
    [EndpointDescription("Adds a new department to a health care facility.")]
    [EndpointName("AdminCreateDepartment")]
    public async Task<IActionResult> CreateDepartment(
        [FromBody] CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Department by Id.")]
    [EndpointDescription("Retrieves a single department.")]
    [EndpointName("AdminGetDepartmentById")]
    public async Task<IActionResult> GetDepartmentById(
        Guid id,
        string? fields,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Departments.")]
    [EndpointDescription("Retrieves departments with optional filtering and pagination.")]
    [EndpointName("AdminGetDepartments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> GetDepartments(
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{Roles.HealthCareFacility}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Updates an existing Department.")]
    [EndpointDescription("Updates the details of an existing department.")]
    [EndpointName("AdminUpdateDepartment")]
    public async Task<IActionResult> UpdateDepartment(
        Guid id,
        [FromBody] UpdateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpPatch("{id:guid}/activate")]
    [Authorize(Roles = $"{Roles.HealthCareFacility}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Activates a Department.")]
    [EndpointDescription("Activates a previously deactivated department.")]
    [EndpointName("AdminActivateDepartment")]
    public async Task<IActionResult> ActivateDepartment(
        Guid id,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpPatch("{id:guid}/deactivate")]
    [Authorize(Roles = $"{Roles.HealthCareFacility}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Deactivates a Department.")]
    [EndpointDescription("Deactivates a department (soft delete).")]
    [EndpointName("AdminDeactivateDepartment")]
    public async Task<IActionResult> DeactivateDepartment(
        Guid id,
        CancellationToken cancellationToken)
    {
        return Ok();
    }



    private List<LinkDto> CreateLinks(string id, string? fields)
    {
        return new List<LinkDto>
        {
            linkService.Create(nameof(GetDepartmentById), "self", HttpMethods.Get, new { id, fields }),
            linkService.Create(nameof(CreateDepartment), "create", HttpMethods.Post),
            linkService.Create(nameof(UpdateDepartment), "update", HttpMethods.Put, new { id }),
            linkService.Create(nameof(ActivateDepartment), "activate", HttpMethods.Patch, new { id }),
            linkService.Create(nameof(DeactivateDepartment), "deactivate", HttpMethods.Patch, new { id }),
            linkService.Create(nameof(GetDepartments), "all", HttpMethods.Get)
        };
    }
}