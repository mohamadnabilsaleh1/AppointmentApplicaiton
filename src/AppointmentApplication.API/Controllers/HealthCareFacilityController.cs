using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Contracts.Requests;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Shared.Results;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace AppointmentApplication.API.Controllers;

[Route("api/healthCareFacilities")]
public sealed class HealthCareFacilityController(ISender sender) : ApiController
{

    [HttpPost]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Creates a new Health Care Facility.")]
    [EndpointDescription("Adds a new Health Care Facility to the system.")]
    [EndpointName("CreateHealthCareFacility")]
    public async Task<IActionResult> CreateHealthCareFacility([FromBody] CreateHealthcareFacilityRequest request, CancellationToken cancellationToken)
    {
        string userId = "wokdflsjklsdf32r";
        Result<HealthCareFacility> result = await sender.Send(
            new CreateHealthcareFacilityCommand(
            userId,
            request.Name,
            request.Type,
            request.Street,
            request.City,
            State: request.State,
            request.Country,
            request.ZipCode,
            request.GPSLatitude,
            request.GPSLatitude
        ), cancellationToken);

        return result.Match(
            response => CreatedAtRoute(
                routeName: "GetCustomerById",
                routeValues: new { version = "1.0", HealthCareFacilityId = response.Id },
                value: response),
            Problem);
    }
    // [HttpGet]
    // [MapToApiVersion("0.1")]
    // [EndpointSummary("Get  Health Care Facilities")]
    // [EndpointDescription("Get Health Care Facilities from the system.")]
    // [EndpointName("GetHealthCareFacilities")]
    // public async Task<IActionResult> GetHealthCareFacilities()
    // {

    // }
}

