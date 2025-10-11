using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Patients.ChronicDiseases.Commands.DeleteChronicDisease;
using AppointmentApplication.Application.Features.Patients.ChronicDiseases.Queries.GetChronicDiseases;
using AppointmentApplication.Application.Features.Patients.Commands.AddAllergy;
using AppointmentApplication.Contracts.Requests.Patients.ChronicDiseases;
using AppointmentApplication.Domain.Users;

using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers.Patients.ChronicDiseases
{
    [Route("api/patients/me/chronic-diseases")]
    [Authorize(Roles = Roles.Patient)]

    public sealed class ChronicDiseaseController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;
        private readonly IUserContext _userContext;

        public ChronicDiseaseController(ISender sender, LinkService linkService, IUserContext userContext)
        {
            _sender = sender;
            _linkService = linkService;
            _userContext = userContext;
        }

        // 🩺 GET: Get all chronic diseases
        [HttpGet]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Get all chronic diseases for the authenticated patient")]
        [EndpointDescription("Retrieves all chronic diseases associated with the currently authenticated patient.")]
        [EndpointName("GetPatientChronicDiseases")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetChronicDiseases(CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;

            var result = await _sender.Send(new GetChronicDiseasesQuery(userId), cancellationToken);
            return result.Match(
                diseases =>
                {
                    var links = CreateLinks(null);
                    var resource = new { data = diseases, links };
                    return Ok(resource);
                },
                Problem);
        }

        // ➕ POST: Add a new chronic disease
        [HttpPost]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Add a new chronic disease for the authenticated patient")]
        [EndpointDescription("Adds a new chronic disease record for the currently authenticated patient.")]
        [EndpointName("AddPatientChronicDisease")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddChronicDisease(
            [FromBody] CreateChronicDisease request,
            CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;
            var result = await _sender.Send(new AddChronicDiseaseCommand(request.ChronicDisease, userId), cancellationToken);

            return result.Match(
                disease =>
                {
                    var links = CreateLinks(disease.Id.ToString());
                    var resource = new { data = disease, links };

                    return CreatedAtAction(
                        nameof(GetChronicDiseases),
                        new { id = disease.Id },
                        resource);
                },
                Problem);
        }

        // ❌ DELETE: Delete a chronic disease
        [HttpDelete("{id:guid}")]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Delete a chronic disease for the authenticated patient")]
        [EndpointDescription("Removes a specific chronic disease record for the currently authenticated patient.")]
        [EndpointName("DeletePatientChronicDisease")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteChronicDisease(Guid id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new DeleteChronicDiseaseCommand(_userContext.UserId, id), cancellationToken);
            return result.Match<IActionResult>(_ => NoContent(), Problem);
        }

        // 🔗 HATEOAS link builder
        private List<LinkDto> CreateLinks(string? id)
        {
            var links = new List<LinkDto>
            {
                _linkService.Create(nameof(GetChronicDiseases), "self", HttpMethods.Get)
            };

            if (id is not null)
            {
                links.Add(_linkService.Create(nameof(AddChronicDisease), "create", HttpMethods.Post));
                links.Add(_linkService.Create(nameof(DeleteChronicDisease), "delete", HttpMethods.Delete, new { id }));
            }

            return links;
        }
    }
}
