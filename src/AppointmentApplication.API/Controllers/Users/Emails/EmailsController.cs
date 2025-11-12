// AppointmentApplication.API/Controllers/EmailsController.cs
using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Emails.Commands;
using AppointmentApplication.Application.Features.Emails.Commands.AddEmail;
using AppointmentApplication.Application.Features.Emails.Commands.RemoveEmail;
using AppointmentApplication.Application.Features.Emails.Commands.SetPrimaryEmail;
using AppointmentApplication.Application.Features.Emails.Commands.UpdateEmail;
using AppointmentApplication.Application.Features.Emails.Queries;
using AppointmentApplication.Application.Features.Emails.Queries.GetUserEmails;
using AppointmentApplication.Contracts.Requests.Emails;
using AppointmentApplication.Domain.Shared.Results;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers
{
    [Route("api/users/me/emails")]
    // [Authorize]
    public sealed class EmailsController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;
        private readonly IUserContext _userContext;

        public EmailsController(ISender sender, LinkService linkService, IUserContext userContext)
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
        [EndpointName("GetMyEmails")]
        [EndpointSummary("Get current user's emails")]
        [EndpointDescription("Retrieves all email addresses for the currently authenticated user.")]
        public async Task<IActionResult> GetMyEmails(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetUserEmailsQuery(_userContext.UserId), cancellationToken);

            return result.Match(
                emails =>
                {
                    var links = CreateLinks();
                    var resource = new { data = emails, links };
                    return Ok(resource);
                },
                Problem);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointName("AddEmail")]
        [EndpointSummary("Add new email")]
        [EndpointDescription("Adds a new email address for the currently authenticated user.")]
        public async Task<IActionResult> AddEmail(
            [FromBody] AddEmailRequest request,
            CancellationToken cancellationToken)
        {
            var command = new AddEmailCommand(_userContext.UserId, request.EmailAddress, request.Label, request.IsPrimary);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                email =>
                {
                    var links = CreateLinks(email.Id);
                    var resource = new { data = email, links };
                    return CreatedAtAction(nameof(GetMyEmails), new { id = email.Id }, resource);
                },
                Problem);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointName("UpdateEmail")]
        [EndpointSummary("Update email")]
        [EndpointDescription("Updates an existing email address for the currently authenticated user.")]
        public async Task<IActionResult> UpdateEmail(
            Guid id,
            [FromBody] UpdateEmailRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateEmailCommand(_userContext.UserId, id, request.EmailAddress, request.Label, request.IsPrimary);

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
        [EndpointName("SetPrimaryEmail")]
        [EndpointSummary("Set email as primary")]
        [EndpointDescription("Sets an email address as the primary contact for the currently authenticated user.")]
        public async Task<IActionResult> SetPrimaryEmail(Guid id, CancellationToken cancellationToken)
        {
            // First get the email to get its address

            var command = new SetPrimaryEmailCommand(_userContext.UserId, id);
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
        [EndpointName("DeleteEmail")]
        [EndpointSummary("Delete email")]
        [EndpointDescription("Deletes an email address for the currently authenticated user.")]
        public async Task<IActionResult> DeleteEmail(Guid id, CancellationToken cancellationToken)
        {
            // First get the email to get its address

            var command = new RemoveEmailCommand(_userContext.UserId, id);
            var result = await _sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                Problem);
        }

        private List<LinkDto> CreateLinks(Guid? emailId = null)
        {
            var links = new List<LinkDto>
            {
                _linkService.Create(nameof(GetMyEmails), "all", HttpMethods.Get),
            };

            if (emailId.HasValue)
            {
                links.Add(_linkService.Create(nameof(UpdateEmail), "update", HttpMethods.Put, new { id = emailId.Value }));
                links.Add(_linkService.Create(nameof(SetPrimaryEmail), "set-primary", HttpMethods.Put, new { id = emailId.Value }));
                links.Add(_linkService.Create(nameof(DeleteEmail), "delete", HttpMethods.Delete, new { id = emailId.Value }));
            }
            else
            {
                links.Add(_linkService.Create(nameof(AddEmail), "add", HttpMethods.Post));
            }

            return links;
        }
    }
}