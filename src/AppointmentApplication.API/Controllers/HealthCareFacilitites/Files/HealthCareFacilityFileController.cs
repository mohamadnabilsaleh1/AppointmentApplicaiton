using System;
using System.Collections.Generic;
using System.IO;
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
    [Route("api/files/health-care-facilities/{facilityId:guid}/uploads")]
    public class HealthCareFacilityFileController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;
        private readonly IUserContext _userContext;

        public HealthCareFacilityFileController(
            ISender sender,
            LinkService linkService,
            IUserContext userContext)
        {
            _sender = sender;
            _linkService = linkService;
            _userContext = userContext;
        }

        // 🔹 Get upload file content by ID
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointName("GetHealthCareFacilityUploadeadFileById")]
        [EndpointSummary("Retrieve a specific uploaded file content by ID.")]
        [EndpointDescription("Fetches and returns the actual file content (image, PDF, etc.) for a specific uploaded file of a healthcare facility.")]
        public async Task<IActionResult> GetFileById(Guid facilityId, Guid id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetFileContentByIdQuery(facilityId, id), cancellationToken);

            return result.Match(
                fileResult =>
                {
                    // For file downloads, we return the actual file content
                    // No HATEOAS links needed since this returns raw file data
                    return File(fileResult.Content, fileResult.ContentType, fileResult.FileDownloadName);
                },
                Problem);
        }

        // 🔹 Get upload metadata by ID (if you need metadata with HATEOAS)

        // 🔹 Helper: HATEOAS links
        private List<LinkDto> CreateLinks(Guid facilityId, Guid? uploadId = null)
        {
            var links = new List<LinkDto>
            {
                _linkService.Create(nameof(GetFileById), "self", HttpMethods.Get, new { facilityId, id = uploadId }),
            };
            return links;
        }
    }
}