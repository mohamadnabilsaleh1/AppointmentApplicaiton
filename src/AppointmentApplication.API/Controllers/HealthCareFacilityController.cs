using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Dtos.HealthCareFacilities;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilities;
using AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilityById;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Contracts.Requests;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Shared.Results;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/healthCareFacilities")]
public sealed class HealthCareFacilityController(ISender sender) : ApiController
{
    // [HttpPost]
    // [ProducesResponseType(typeof(HealthcareFacilityDto), StatusCodes.Status201Created)]
    // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    // [MapToApiVersion("0.1")]
    // [EndpointSummary("Creates a new Health Care Facility.")]
    // [EndpointDescription("Adds a new Health Care Facility to the system.")]
    // [EndpointName("CreateHealthCareFacility")]
    // public async Task<IActionResult> CreateHealthCareFacility([FromBody] CreateHealthcareFacilityRequest request, CancellationToken cancellationToken)
    // {
    //     string userId = "wokdflsjklsdf32r";

    //     Result<HealthCareFacility> result = await sender.Send(
    //         new CreateHealthcareFacilityCommand(
    //             userId,
    //             request.Name,
    //             request.Type,
    //             request.Street,
    //             request.City,
    //             request.State,
    //             request.Country,
    //             request.ZipCode,
    //             request.GPSLatitude,
    //             request.GPSLongitude
    //         ),
    //         cancellationToken
    //     );

    //     return result.Match(
    //         response => CreatedAtRoute(
    //             routeName: "GetHealthCareFacilityById",
    //             routeValues: new { id = response.Id, apiVersion = "0.1" }, // ✅ Fixed parameter name
    //             value: response),
    //         Problem);
    // }

    [HttpGet("{id:guid}", Name = "GetHealthCareFacilityById")]
    [ProducesResponseType(typeof(HealthcareFacilityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Health Care Facility by Id.")]
    [EndpointDescription("Retrieves a single Health Care Facility.")]
    [EndpointName("GetHealthCareFacilityById")]
    public async Task<IActionResult> GetHealthCareFacilityById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetHealthCareFacilityByIdQuery(id), cancellationToken);

        return result.Match(
            facility => Ok(facility),
            Problem
        );
    }


    [HttpGet]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get Health Care Facilities.")]
    [EndpointDescription("Retrieves health care facilities with optional search and pagination.")]
    [EndpointName("GetHealthCareFacilities")]
    [ProducesResponseType(typeof(PaginationResult<HealthcareFacilityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> GetHealthCareFacilities(
    [FromQuery] HealthCareFacilityQueryParameters queryParameters,
    CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetHealthCareFacilityQuery(
                queryParameters.Search,
                queryParameters.Page,
                queryParameters.PageSize,
                queryParameters.Sort,
                queryParameters.Fields,
                queryParameters.Type,
                queryParameters.Street,
                queryParameters.City,
                queryParameters.State,
                queryParameters.Country,
                queryParameters.ZipCode
                ),
            cancellationToken
        );
        return result.Match(
            response => Ok(response),
            Problem
        );
    }


}

