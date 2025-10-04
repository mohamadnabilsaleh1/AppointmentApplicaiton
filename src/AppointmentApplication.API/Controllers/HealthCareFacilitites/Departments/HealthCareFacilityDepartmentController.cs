using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Contracts.Requests.Departments;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers;

[Route("api/health-care-facilities/{facilityId:guid}/departments")]
public sealed class HealthCareFacilityDepartmentController : ApiController
{
    private readonly ISender _sender;
    private readonly LinkService _linkService;

    public HealthCareFacilityDepartmentController(ISender sender, LinkService linkService)
    {
        _sender = sender;
        _linkService = linkService;
    }

    // GET all departments for the logged-in facility
    [HttpGet(Name = "GetDepartments")]
    [MapToApiVersion("0.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> GetDepartments(CancellationToken cancellationToken)
    {
        // استدعاء _sender.Send() لجلب البيانات من DB
        return Ok(); // placeholder
    }

    // GET department by Id
    [HttpGet("{id:guid}", Name = "GetDepartmentById")]
    [MapToApiVersion("0.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDepartmentById(Guid id, string? fields, CancellationToken cancellationToken)
    {
        // استدعاء _sender.Send() لجلب البيانات حسب Id
        return Ok(); // placeholder
    }

    private List<LinkDto> CreateLinks(Guid? id = null, string? fields = null)
    {
        var links = new List<LinkDto>
        {
            _linkService.Create(nameof(GetDepartments), "self", HttpMethods.Get),
            _linkService.Create(nameof(GetDepartmentById), "self", HttpMethods.Get, new { id, fields })
        };
        return links;
    }
}
