using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Patients.Allergies.Commands.DeleteAllergy;
using AppointmentApplication.Application.Features.Patients.Allergies.Queries.GetAllergies;
using AppointmentApplication.Application.Features.Patients.Commands.AddAllergy;
using AppointmentApplication.Contracts.Requests.Patients.Allergies;
using AppointmentApplication.Domain.Users;

using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers.Patients.Allergies
{
    [Route("api/patients/me/allergies")]
    [Authorize(Roles = Roles.Patient)]
    public sealed class AllergyController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;
        private readonly IUserContext _userContext;

        public AllergyController(ISender sender, LinkService linkService, IUserContext userContext)
        {
            _sender = sender;
            _linkService = linkService;
            _userContext = userContext;
        }

        // 🩺 GET: Get all allergies
        [HttpGet]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Get all allergies for the authenticated patient")]
        [EndpointDescription("Retrieves all allergies associated with the currently authenticated patient.")]
        [EndpointName("GetPatientAllergies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetAllergies(CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;

            var result = await _sender.Send(new GetAllergiesQuery(userId), cancellationToken);
            return result.Match(
                allergies =>
                {
                    var links = CreateLinks(null);
                    var resource = new { data = allergies, links };
                    return Ok(resource);
                },
                Problem);
        }

        // ➕ POST: Add a new allergy
        [HttpPost]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Add a new allergy for the authenticated patient")]
        [EndpointDescription("Adds a new allergy record for the currently authenticated patient.")]
        [EndpointName("AddPatientAllergy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAllergy(
            [FromBody] CreateAllergy request,
            CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;
            var result = await _sender.Send(new AddAllergyCommand(request.Allergy, userId), cancellationToken);

            return result.Match(
                allergy =>
                {
                    var links = CreateLinks(allergy.Id.ToString());
                    var resource = new { data = allergy, links };

                    return CreatedAtAction(
                        nameof(GetAllergies),
                        new { id = allergy.Id },
                        resource);
                },
                Problem);
        }

        // ❌ DELETE: Delete an allergy
        [HttpDelete("{id:guid}")]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Delete an allergy for the authenticated patient")]
        [EndpointDescription("Removes a specific allergy record for the currently authenticated patient.")]
        [EndpointName("DeletePatientAllergy")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAllergy(Guid id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new DeleteAllergyCommand(_userContext.UserId, id), cancellationToken);
            return result.Match<IActionResult>(_ => NoContent(), Problem);
        }

        // 🔗 HATEOAS link builder
        private List<LinkDto> CreateLinks(string? id)
        {
            var links = new List<LinkDto>
            {
                _linkService.Create(nameof(GetAllergies), "self", HttpMethods.Get)
            };

            if (id is not null)
            {
                links.Add(_linkService.Create(nameof(AddAllergy), "create", HttpMethods.Post));
                links.Add(_linkService.Create(nameof(DeleteAllergy), "delete", HttpMethods.Delete, new { id }));
            }

            return links;
        }
    }
}
