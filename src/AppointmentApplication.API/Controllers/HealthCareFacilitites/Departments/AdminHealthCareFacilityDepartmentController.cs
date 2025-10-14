using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Commands;
using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Commands.DeleteDepartment;
using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Commands.UpdateDepartment;
using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Queries.GetDepartmentById;
using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Queries.GetDepartmentByUserId;
using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Queries.GetDepartments;
using AppointmentApplication.Application.Features.HealthcareFacilities.DepatmentDoctors.Commands.AddDoctorToDepartment;
using AppointmentApplication.Application.Features.HealthcareFacilities.DepatmentDoctors.Commands.DeleteDoctorFromDepartment;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Queries.GetSchedulesByIdQuery;
using AppointmentApplication.Contracts.Requests.Departments;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
namespace AppointmentApplication.API.Controllers
{
    [Route("api/health-care-facilities/me/departments")]
    [Authorize(Roles = Roles.HealthCareFacility)]
    public sealed class AdminHealthCareFacilityDepartmentController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;
        private readonly IUserContext _userContext;
        public AdminHealthCareFacilityDepartmentController(ISender sender, LinkService linkService, IUserContext userContext)
        {
            _sender = sender;
            _linkService = linkService;
            _userContext = userContext;
        }

        [HttpPost]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EndpointName("AdminCreateDepartment")]
        [EndpointSummary("Creates a new Department.")]
        [EndpointDescription("Adds a new department to a health care facility.")]
        public async Task<IActionResult> CreateDepartment(
            [FromBody] CreateDepartmentRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new CreateDepartmentCommand(_userContext.UserId, request.Name, request.Description), cancellationToken);

            return result.Match(
                department =>
                {
                    var links = CreateLinks(department.Id.ToString(), null);
                    var resource = new { data = department, links };

                    return CreatedAtAction(
                        nameof(GetDepartmentById),
                        new { id = department.Id },
                        resource);
                },
                Problem);
        }
        [HttpPost("{departmentId:guid}/doctors/{doctorId:guid}")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EndpointName("AddDoctorToDepartment")]
        [EndpointSummary("Adds a doctor to a department.")]
        [EndpointDescription("Associates a doctor with a specific department.")]
        public async Task<IActionResult> AddDoctorToDepartment(
    [FromRoute] Guid departmentId,
    [FromRoute] Guid doctorId,
    CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new AddDoctorToDepartmentCommand(_userContext.UserId, doctorId, departmentId), cancellationToken);

            return result.Match<IActionResult>(
                _ => CreatedAtAction(
                    nameof(AddDoctorToDepartment),
                    new { departmentId, doctorId },
                    new { departmentId, doctorId }),
                Problem);
        }
        [HttpDelete("{departmentId:guid}/doctors/{doctorId:guid}")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EndpointName("RemoveDoctorFromDepartment")]
        [EndpointSummary("Removes a doctor from a department.")]
        [EndpointDescription("Disassociates a doctor from a specific department.")]
        public async Task<IActionResult> RemoveDoctorFromDepartment(
    [FromRoute] Guid departmentId,
    [FromRoute] Guid doctorId,
    CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new DeleteDoctorFromDepartmentCommand(_userContext.UserId, doctorId, departmentId), cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                Problem);
        }
        
        [HttpGet("{departmentId:guid}/doctors")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [OutputCache(Duration = 60)]
        [EndpointName("AdminGetDoctorsOfDepartments")]
        [EndpointSummary("Get Departments.")]
        [EndpointDescription("Retrieves departments with optional filtering and pagination.")]
        public async Task<IActionResult> GetDoctorsOfDepartment(
            Guid departmentId,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetDoctorsByUserIdQuery(_userContext.UserId, departmentId), cancellationToken);
            return result.Match(
                schedules =>
                {
                    var resource = new { data = schedules };
                    return Ok(resource);
                },
                Problem);
        }


        [HttpGet("{id:guid}")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EndpointName("AdminGetDepartmentById")]
        [EndpointSummary("Get Department by Id.")]
        [EndpointDescription("Retrieves a single department.")]
        public async Task<IActionResult> GetDepartmentById(
            Guid id,
            string? fields,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetDepartmentByUserIdQuery(_userContext.UserId, id), cancellationToken);
            return result.Match(
                schedule =>
                {
                    var links = CreateLinks(id.ToString(), null);
                    var resource = new { data = schedule, links };
                    return Ok(resource);
                },
                Problem);
        }

        [HttpGet]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [OutputCache(Duration = 60)]
        [EndpointName("AdminGetDepartments")]
        [EndpointSummary("Get Departments.")]
        [EndpointDescription("Retrieves departments with optional filtering and pagination.")]
        public async Task<IActionResult> GetDepartments(
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetDepartmentsByUserIdQuery(_userContext.UserId), cancellationToken);
            return result.Match(
                schedules =>
                {
                    var resource = new { data = schedules };
                    return Ok(resource);
                },
                Problem);
        }

        [HttpPut("{id:guid}")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EndpointName("AdminUpdateDepartment")]
        [EndpointSummary("Updates an existing Department.")]
        [EndpointDescription("Updates the details of an existing department.")]
        public async Task<IActionResult> UpdateDepartment(
            Guid id,
            [FromBody] UpdateDepartmentRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new UpdateDepartmentCommand(_userContext.UserId, id, request.Name, request.Description), cancellationToken);
            return result.Match<IActionResult>(_ => NoContent(), Problem);
        }

        [HttpDelete("{id:guid}")]
        [MapToApiVersion("0.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EndpointName("AdminDeleteDepartment")]
        [EndpointSummary("Deletes a Department.")]
        [EndpointDescription("Deletes a department.")]
        public async Task<IActionResult> DeleteDepartment(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new DeleteDepartmentCommand(_userContext.UserId, id), cancellationToken);
            return result.Match<IActionResult>(_ => NoContent(), Problem);
        }

        private List<LinkDto> CreateLinks(string id, string? fields)
        {
            return new List<LinkDto>
            {
                _linkService.Create(nameof(GetDepartmentById), "self", HttpMethods.Get, new { id, fields }),
                _linkService.Create(nameof(CreateDepartment), "create", HttpMethods.Post),
                _linkService.Create(nameof(UpdateDepartment), "update", HttpMethods.Put, new { id }),
                _linkService.Create(nameof(DeleteDepartment), "delete", HttpMethods.Delete, new { id }),
                _linkService.Create(nameof(GetDepartments), "all", HttpMethods.Get)
            };
        }
    }
}
