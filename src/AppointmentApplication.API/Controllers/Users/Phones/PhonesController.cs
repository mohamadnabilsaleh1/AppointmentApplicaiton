// AppointmentApplication.API/Controllers/PhonesController.cs
using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Phones.Commands;
using AppointmentApplication.Application.Features.Phones.Commands.AddPhone;
using AppointmentApplication.Application.Features.Phones.Commands.RemovePhone;
using AppointmentApplication.Application.Features.Phones.Commands.SetPrimaryPhone;
using AppointmentApplication.Application.Features.Phones.Commands.UpdatePhone;
using AppointmentApplication.Application.Features.Phones.Queries;
using AppointmentApplication.Application.Features.Phones.Queries.GetUserPhones;
using AppointmentApplication.Contracts.Requests.Phones;
using AppointmentApplication.Domain.Shared.Results;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers
{
    [Route("api/users/me/phones")]
    [Authorize]
    public sealed class PhonesController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;
        private readonly IUserContext _userContext;

        public PhonesController(ISender sender, LinkService linkService, IUserContext userContext)
        {
            _sender = sender;
            _linkService = linkService;
            _userContext = userContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [OutputCache(Duration = 30)]
        [EndpointName("GetMyPhones")]
        [EndpointSummary("Get current user's phones")]
        [EndpointDescription("Retrieves all phone numbers for the currently authenticated user.")]
        public async Task<IActionResult> GetMyPhones(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetUserPhonesQuery(_userContext.UserId), cancellationToken);

            return result.Match(
                phones =>
                {
                    var links = CreateLinks();
                    var resource = new { data = phones, links };
                    return Ok(resource);
                },
                Problem);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointName("AddPhone")]
        [EndpointSummary("Add new phone")]
        [EndpointDescription("Adds a new phone number for the currently authenticated user.")]
        public async Task<IActionResult> AddPhone(
            [FromBody] AddPhoneRequest request,
            CancellationToken cancellationToken)
        {
            var command = new AddPhoneCommand(_userContext.UserId, request.PhoneNumber, request.Label, request.IsPrimary);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                phone =>
                {
                    var links = CreateLinks(phone.Id);
                    var resource = new { data = phone, links };
                    return CreatedAtAction(nameof(GetMyPhones), new { id = phone.Id }, resource);
                },
                Problem);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointName("UpdatePhone")]
        [EndpointSummary("Update phone")]
        [EndpointDescription("Updates an existing phone number for the currently authenticated user.")]
        public async Task<IActionResult> UpdatePhone(
            Guid id,
            [FromBody] UpdatePhoneRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdatePhoneCommand(_userContext.UserId, id, request.PhoneNumber, request.Label, request.IsPrimary);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                Problem);
        }

        [HttpPut("{id:guid}/primary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointName("SetPrimaryPhone")]
        [EndpointSummary("Set phone as primary")]
        [EndpointDescription("Sets a phone number as the primary contact for the currently authenticated user.")]
        public async Task<IActionResult> SetPrimaryPhone(Guid id, CancellationToken cancellationToken)
        {

            var command = new SetPrimaryPhoneCommand(_userContext.UserId, id);
            var result = await _sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                Problem);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointName("DeletePhone")]
        [EndpointSummary("Delete phone")]
        [EndpointDescription("Deletes a phone number for the currently authenticated user.")]
        public async Task<IActionResult> DeletePhone(Guid id, CancellationToken cancellationToken)
        {
            // First get the phone to get its number

            var command = new RemovePhoneCommand(_userContext.UserId, id);
            var result = await _sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                Problem);
        }

        private List<LinkDto> CreateLinks(Guid? phoneId = null)
        {
            var links = new List<LinkDto>
            {
                _linkService.Create(nameof(GetMyPhones), "all", HttpMethods.Get),
            };

            if (phoneId.HasValue)
            {
                links.Add(_linkService.Create(nameof(UpdatePhone), "update", HttpMethods.Put, new { id = phoneId.Value }));
                links.Add(_linkService.Create(nameof(SetPrimaryPhone), "set-primary", HttpMethods.Put, new { id = phoneId.Value }));
                links.Add(_linkService.Create(nameof(DeletePhone), "delete", HttpMethods.Delete, new { id = phoneId.Value }));
            }
            else
            {
                links.Add(_linkService.Create(nameof(AddPhone), "add", HttpMethods.Post));
            }

            return links;
        }
    }
}