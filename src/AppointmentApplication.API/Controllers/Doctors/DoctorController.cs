using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Dtos.HealthCareFacilities;
using AppointmentApplication.API.Services;
using AppointmentApplication.Contracts.Requests.Doctors;


using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/doctors")]
public sealed class DoctorController(ISender sender, LinkService linkService) : ApiController
{
    [HttpGet("{id:guid}", Name = "GetDoctorById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Doctor by Id.")]
    [EndpointDescription("Retrieves a single doctor.")]
    [EndpointName("GetDoctorById")]
    public async Task<IActionResult> GetDoctorById(
        Guid id,
        string? fields,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Doctors.")]
    [EndpointDescription("Retrieves doctors with optional filtering and pagination.")]
    [EndpointName("GetDoctors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> GetDoctors(
        [FromQuery] DoctorQueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        return Ok();
    }


    [HttpGet("me", Name = "GetMyDoctorProfile")]
    [Authorize(Roles = Roles.Doctor)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get current logged-in Doctor's profile.")]
    [EndpointDescription("Retrieves the profile of the currently authenticated doctor.")]
    [EndpointName("GetMyDoctorProfile")]
    public async Task<IActionResult> GetMyDoctorProfile(
    string? fields,
    CancellationToken cancellationToken)
    {

        return Ok();
    }

    [HttpPut("me", Name = "UpdateMyDoctorProfile")]
    [Authorize(Roles = Roles.Doctor)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Updates current logged-in Doctor's profile.")]
    [EndpointDescription("Updates the profile of the currently authenticated doctor.")]
    [EndpointName("UpdateMyDoctorProfile")]
    public async Task<IActionResult> UpdateMyDoctorProfile(
    [FromBody] UpdateDoctorRequest request,
    CancellationToken cancellationToken)
    {

        return Ok();
    }


    private List<LinkDto> CreateLinks(string id, string? fields)
    {
        var links = new List<LinkDto>
        {
        linkService.Create(nameof(GetDoctorById), "self", HttpMethods.Get, new { id, fields }),
        linkService.Create(nameof(GetDoctors), "all", HttpMethods.Get),
        linkService.Create(nameof(GetMyDoctorProfile), "my-profile", HttpMethods.Get),
        linkService.Create(nameof(UpdateMyDoctorProfile), "update-my-profile", HttpMethods.Put),
        };
        return links;
    }

    private List<LinkDto> CreateLinks(DoctorQueryParameters parameters, bool hasNextPage, bool hasPreviousPage)
    {
        List<LinkDto> links = new()
        {
            linkService.Create(nameof(GetDoctors), "self", HttpMethods.Get, new
            {
                page = parameters.Page,
                pageSize = parameters.PageSize,
                fields = parameters.Fields,
                search = parameters.Search,
                sort = parameters.Sort,
                facilityId = parameters.FacilityId,
                departmentId = parameters.DepartmentId,
                specializationId = parameters.SpecializationId,
                gender = parameters.Gender,
                isActive = parameters.IsActive
            }),
        };

        if (hasNextPage)
        {
            links.Add(linkService.Create(nameof(GetDoctors), "next-page", HttpMethods.Get, new
            {
                page = parameters.Page + 1,
                pageSize = parameters.PageSize,
                fields = parameters.Fields,
                search = parameters.Search,
                sort = parameters.Sort,
                facilityId = parameters.FacilityId,
                departmentId = parameters.DepartmentId,
                specializationId = parameters.SpecializationId,
                gender = parameters.Gender,
                isActive = parameters.IsActive
            }));
        }

        if (hasPreviousPage)
        {
            links.Add(linkService.Create(nameof(GetDoctors), "previous-page", HttpMethods.Get, new
            {
                page = parameters.Page - 1,
                pageSize = parameters.PageSize,
                fields = parameters.Fields,
                search = parameters.Search,
                sort = parameters.Sort,
                facilityId = parameters.FacilityId,
                departmentId = parameters.DepartmentId,
                specializationId = parameters.SpecializationId,
                gender = parameters.Gender,
                isActive = parameters.IsActive
            }));
        }

        return links;
    }
}