using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Patients.Commands.UpdatePatient;
using AppointmentApplication.Application.Features.Patients.Dtos;
using AppointmentApplication.Application.Features.Patients.Queries.GetPatientByUserId;
using AppointmentApplication.Contracts.Requests.Patient;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace AppointmentApplication.API.Controllers.Patients;

[Route("api/patients")]
[Authorize(Roles = Roles.Patient)]
public sealed class PatientController : ApiController
{
    private readonly ISender _sender;
    private readonly LinkService _linkService;
    private readonly IUserContext _userContext; // للحصول على الـ UserId الحالي

    public PatientController(ISender sender, LinkService linkService, IUserContext userContext)
    {
        _sender = sender;
        _linkService = linkService;
        _userContext = userContext;
    }

    [HttpGet("me")]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [MapToApiVersion("0.1")]
    [EndpointSummary("Get current patient information.")]
    [EndpointDescription("Retrieves the profile information of the currently authenticated patient.")]
    [EndpointName("GetCurrentPatient")]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPatientByUserIdQuery(_userContext.UserId), cancellationToken);
        return result.Match(
            patient =>
            {
                var resource = new
                {
                    data = patient
                };
                return Ok(resource);
            },
            Problem);
    }

//     [HttpPut("me", Name = "UpdateMyDoctorProfile")]
//     [Authorize(Roles = Roles.Doctor)]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     [MapToApiVersion("0.1")]
//     [EndpointSummary("Updates current logged-in Doctor's profile.")]
//     [EndpointDescription("Updates the profile of the currently authenticated doctor.")]
//     [EndpointName("UpdateMyDoctorProfile")]
//     public async Task<IActionResult> UpdateMyDoctorProfile(
// [FromBody] UpdatePatientRequest request,
// CancellationToken cancellationToken)
//     {
//         var result = await _sender.Send(new UpdatePatientCommand(_userContext.UserId, request.FirstName, request.LastName, request.Gender, request.DateOfBirth), cancellationToken);
//         return result.Match<IActionResult>(_ => NoContent(), Problem);
//     }
}
