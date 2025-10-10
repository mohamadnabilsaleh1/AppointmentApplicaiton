using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Dtos.HealthCareFacilities;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Doctors.Commands.UpdateDoctor;
using AppointmentApplication.Application.Features.Doctors.Queries.GetDoctorByUserId;
using AppointmentApplication.Application.Features.Doctors.Queries.GetDoctors;
using AppointmentApplication.Application.Features.Doctors.Queries.GetDoctorsById;
using AppointmentApplication.Contracts.Requests.Doctors;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/doctors")]
public sealed class DoctorController : ApiController
{
    private readonly ISender _sender;
    private readonly LinkService _linkService;
    private readonly IUserContext _userContext;

    public DoctorController(ISender sender, LinkService linkService, IUserContext userContext)
    {
        _sender = sender;
        _linkService = linkService;
        _userContext = userContext;
    }

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
        var result = await _sender.Send(new GetDoctorByIdQuery(id));
        return result.Match(
            doctor =>
            {
                var links = CreateLinks(id.ToString(), fields);
                var resource = new
                {
                    data = doctor,
                    links
                }
                ;
                return Ok(resource);
            },
            Problem);
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
        var result = await _sender.Send(
            new GetDoctorsQuery(
                queryParameters.Search,
                queryParameters.Page,
                queryParameters.PageSize,
                queryParameters.Sort,
                queryParameters.Fields,
                queryParameters.Specialization),
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
        var result = await _sender.Send(new GetDoctorByUserIdQuery(_userContext.UserId));
        return result.Match(
            doctor =>
            {
                var links = CreateLinks(_userContext.UserId.ToString(), fields);
                var resource = new
                {
                    data = doctor,
                    links
                }
                ;
                return Ok(resource);
            },
            Problem);
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
        var result = await _sender.Send(new UpdateDoctorCommand(_userContext.UserId, request.FirstName, request.LastName, request.Gender, request.DateOfBirth), cancellationToken);
        return result.Match<IActionResult>(_ => NoContent(), Problem);
    }

    private List<LinkDto> CreateLinks(string id, string? fields)
    {
        var links = new List<LinkDto>
        {
        _linkService.Create(nameof(GetDoctorById), "self", HttpMethods.Get, new { id, fields }),
        _linkService.Create(nameof(GetDoctors), "all", HttpMethods.Get),
        _linkService.Create(nameof(GetMyDoctorProfile), "my-profile", HttpMethods.Get),
        _linkService.Create(nameof(UpdateMyDoctorProfile), "update-my-profile", HttpMethods.Put),
        };
        return links;
    }

    private List<LinkDto> CreateLinks(DoctorQueryParameters parameters, bool hasNextPage, bool hasPreviousPage)
    {
        List<LinkDto> links = new()
        {
            _linkService.Create(nameof(GetDoctors), "self", HttpMethods.Get, new
            {
                page = parameters.Page,
                pageSize = parameters.PageSize,
                fields = parameters.Fields,
                search = parameters.Search,
                sort = parameters.Sort,
                specialization = parameters.Specialization,
            }),
        };

        if (hasNextPage)
        {
            links.Add(_linkService.Create(nameof(GetDoctors), "next-page", HttpMethods.Get, new
            {
                page = parameters.Page + 1,
                pageSize = parameters.PageSize,
                fields = parameters.Fields,
                search = parameters.Search,
                sort = parameters.Sort,
                specialization = parameters.Specialization,

            }));
        }

        if (hasPreviousPage)
        {
            links.Add(_linkService.Create(nameof(GetDoctors), "previous-page", HttpMethods.Get, new
            {
                page = parameters.Page + 1,
                pageSize = parameters.PageSize,
                fields = parameters.Fields,
                search = parameters.Search,
                sort = parameters.Sort,
                specialization = parameters.Specialization,
            }));
        }

        return links;
    }
}
