using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Commands.ChangeFileToPrivate;
using AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Dtos;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Contracts.Requests.Patients.Uploads;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers.HealthCareFacilities.Uploads
{
    [Route("api/health-care-facilities/me/uploads")]
    [Authorize(Roles = Roles.HealthCareFacility)]
    // [ApiVersion("0.1")]
    public sealed class AdminHealthCareFacilityUploadController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;
        private readonly IUserContext _userContext;

        public AdminHealthCareFacilityUploadController(ISender sender, LinkService linkService, IUserContext userContext)
        {
            _sender = sender;
            _linkService = linkService;
            _userContext = userContext;
        }

        // ✅ Create Upload
        [HttpPost]
        [Authorize(Roles = Roles.HealthCareFacility)]
        [ProducesResponseType(typeof(UploadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Create a new upload file for a Health Care Facility.")]
        [EndpointDescription("Uploads a file (e.g., image, document) and associates it with the logged-in Health Care Facility.")]
        [EndpointName("AdminCreateHealthCareFacilityUpload")]
        public async Task<IActionResult> CreateUpload([FromForm] CreateUploadRequest request, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                new CreateUploadFileCommand(_userContext.UserId, request.File, request.Title, request.Description, request.Visibility),
                cancellationToken);

            return result.Match(
                upload =>
                {
                    var links = CreateLinks(upload.Id.ToString(), null);
                    var resource = new { data = upload, links };
                    return CreatedAtAction(nameof(GetUploadById), new { id = upload.Id }, resource);
                },
                Problem);
        }

        // ✅ Get Upload by ID
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UploadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Get an uploaded file by ID.")]
        [EndpointDescription("Retrieves a specific uploaded file belonging to the logged-in Health Care Facility.")]
        [EndpointName("AdminGetHealthCareFacilityUploadById")]
        public async Task<IActionResult> GetUploadById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetUploadedFileByUserIdQuery(_userContext.UserId, id), cancellationToken);
            return result.Match(
                upload =>
                {
                    var links = CreateLinks(id.ToString(), null);
                    var resource = new { data = upload, links };
                    return Ok(resource);
                },
                Problem);
        }

        // ✅ Get All Uploads
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UploadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [OutputCache(Duration = 60)]
        [EndpointSummary("Get all uploads for a Health Care Facility.")]
        [EndpointDescription("Retrieves all uploaded files associated with the authenticated Health Care Facility.")]
        [EndpointName("AdminGetHealthCareFacilityUploads")]
        public async Task<IActionResult> GetUploads(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetUploadedFilesByUserIdQuery(_userContext.UserId), cancellationToken);
            return result.Match(
                uploads =>
                {
                    var resource = new { data = uploads };
                    return Ok(resource);
                },
                Problem);
        }

        // ✅ Update Upload
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Update an uploaded file.")]
        [EndpointDescription("Updates the metadata (title, description) of a specific uploaded file.")]
        [EndpointName("AdminUpdateHealthCareFacilityUpload")]
        public async Task<IActionResult> UpdateUpload(Guid id, [FromBody] UpdateUploadRequest request, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new UpdateUploadFileCommand(_userContext.UserId, id, request.Title, request.Description), cancellationToken);
            return result.Match<IActionResult>(_ => NoContent(), Problem);
        }

        // ✅ Delete Upload
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Delete an uploaded file.")]
        [EndpointDescription("Removes a specific uploaded file belonging to the logged-in Health Care Facility.")]
        [EndpointName("AdminDeleteHealthCareFacilityUpload")]
        public async Task<IActionResult> DeleteUpload(Guid id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new DeleteUploadedFileCommand(_userContext.UserId, id), cancellationToken);
            return result.Match<IActionResult>(_ => NoContent(), Problem);
        }

        // ✅ Change file visibility to PUBLIC
        [HttpPatch("{id:guid}/make-public")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Make uploaded file public.")]
        [EndpointDescription("Changes file visibility to public, allowing anyone to access it.")]
        [EndpointName("AdminMakeHealthCareFacilityUploadPublic")]
        public async Task<IActionResult> MakeUploadPublic(Guid id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new ChangeFileToPublicCommand(_userContext.UserId, id), cancellationToken);
            return result.Match<IActionResult>(_ => NoContent(), Problem);
        }

        // ✅ Change file visibility to PRIVATE
        [HttpPatch("{id:guid}/make-private")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Make uploaded file private.")]
        [EndpointDescription("Restricts access to the uploaded file, making it visible only to the Health Care Facility.")]
        [EndpointName("AdminMakeHealthCareFacilityUploadPrivate")]
        public async Task<IActionResult> MakeUploadPrivate(Guid id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new ChangeFileToPrivateCommand(_userContext.UserId, id), cancellationToken);
            return result.Match<IActionResult>(_ => NoContent(), Problem);
        }

        // 🔗 HATEOAS Links
        private List<LinkDto> CreateLinks(string id, string? fields)
        {
            return new List<LinkDto>
            {
                _linkService.Create(nameof(GetUploadById), "self", HttpMethods.Get, new { id, fields }),
                _linkService.Create(nameof(CreateUpload), "create", HttpMethods.Post),
                _linkService.Create(nameof(UpdateUpload), "update", HttpMethods.Put, new { id }),
                _linkService.Create(nameof(DeleteUpload), "delete", HttpMethods.Delete, new { id }),
                _linkService.Create(nameof(GetUploads), "all", HttpMethods.Get),
                _linkService.Create(nameof(MakeUploadPublic), "make-public", HttpMethods.Patch, new { id }),
                _linkService.Create(nameof(MakeUploadPrivate), "make-private", HttpMethods.Patch, new { id })
            };
        }
    }
}