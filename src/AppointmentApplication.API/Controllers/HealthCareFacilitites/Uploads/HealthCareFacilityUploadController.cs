using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;

using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Commands.ChangeFileToPrivate;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers.HealthCareFacilities.Uploads
{
    [ApiController]
    [Route("api/health-care-facilities/{facilityId:guid}/uploads")]
    public class HealthCareFacilityUploadController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;
        private readonly IUserContext _userContext;

        public HealthCareFacilityUploadController(
            ISender sender,
            LinkService linkService,
            IUserContext userContext)
        {
            _sender = sender;
            _linkService = linkService;
            _userContext = userContext;
        }
        
        // 🔹 Get all uploads
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [OutputCache(Duration = 60)]
        [EndpointName("GetHealthCareFacilityUploads")]
        [EndpointSummary("Retrieve all uploaded files for a healthcare facility.")]
        [EndpointDescription("Gets a list of uploaded files for the specified healthcare facility. Results are cached for 60 seconds.")]
        public async Task<IActionResult> GetAllFiles(Guid facilityId, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetUploadedFilesQuery(facilityId), cancellationToken);

            return result.Match(
                files => Ok(new { data = files }),
                Problem);
        }

        // 🔹 Get upload by ID
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointName("GetHealthCareFacilityFileById")]
        [EndpointSummary("Retrieve a specific uploaded file by ID.")]
        [EndpointDescription("Fetches detailed information for a specific uploaded file of a healthcare facility.")]
        public async Task<IActionResult> GetFileById(Guid facilityId, Guid id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetUploadedFileByIdQuery(facilityId, id), cancellationToken);

            return result.Match(
                file =>
                {
                    var links = CreateLinks(facilityId, id);
                    var resource = new { data = file, links };
                    return Ok(resource);
                },
                Problem);
        }

        // 🔹 Helper: HATEOAS links
        private List<LinkDto> CreateLinks(Guid facilityId, Guid? uploadId = null)
        {
            var links = new List<LinkDto>
            {
                _linkService.Create(nameof(GetAllFiles), "all", HttpMethods.Get, new { facilityId }),
                _linkService.Create(nameof(GetFileById), "self", HttpMethods.Get, new { facilityId, id = uploadId })
            };
            return links;
        }
    }
}