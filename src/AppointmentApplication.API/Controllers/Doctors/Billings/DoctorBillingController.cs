using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API.Controllers;
using AppointmentApplication.API.Dtos;
using AppointmentApplication.API.Models.Billings;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Billings.Queries.GetDoctorBillingsByUserId;
using AppointmentApplication.Domain.Users;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppointmentApplication.API.Controllers
{
    [Route("api/doctors/me/billings")]
    public class DoctorBillingController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;
        private readonly IUserContext _userContext;

        public DoctorBillingController(
            ISender sender,
            LinkService linkService,
            IUserContext userContext)
        {
            _sender = sender;
            _linkService = linkService;
            _userContext = userContext;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Doctor)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Get current doctor's billings.")]
        [EndpointDescription("Retrieves paginated billings for the currently authenticated doctor with filtering and sorting.")]
        [EndpointName("GetMyDoctorBillings")]
        [OutputCache(Duration = 30)]
        public async Task<IActionResult> GetMyDoctorBillings(
            [FromQuery] BillingQueryParameters queryParameters,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                new GetDoctorBillingsByUserIdQuery(
                    UserId: _userContext.UserId,
                    StartDate: queryParameters.StartDate,
                    EndDate: queryParameters.EndDate,
                    Status: queryParameters.Status,
                    Search: queryParameters.Search,
                    Sort: queryParameters.Sort,
                    Page: queryParameters.Page,
                    PageSize: queryParameters.PageSize,
                    Fields: queryParameters.Fields),
                cancellationToken);

            return result.Match(
                response =>
                {
                    var hasNextPage = response.Page < response.TotalPages;
                    var hasPreviousPage = response.Page > 1;

                    var links = CreateLinks(queryParameters, hasNextPage, hasPreviousPage);

                    var resource = new
                    {
                        data = response.Items,
                        pagination = new
                        {
                            response.Page,
                            response.PageSize,
                            response.TotalCount,
                            response.TotalPages
                        },
                        links
                    };

                    return Ok(resource);
                },
                Problem);
        }

        private List<LinkDto> CreateLinks(BillingQueryParameters parameters, bool hasNextPage, bool hasPreviousPage)
        {
            List<LinkDto> links = new()
            {
                _linkService.Create(nameof(GetMyDoctorBillings), "self", HttpMethods.Get, new
                {
                    page = parameters.Page,
                    pageSize = parameters.PageSize,
                    fields = parameters.Fields,
                    search = parameters.Search,
                    sort = parameters.Sort,
                    startDate = parameters.StartDate,
                    endDate = parameters.EndDate,
                    status = parameters.Status
                }),
            };

            if (hasNextPage)
            {
                links.Add(_linkService.Create(nameof(GetMyDoctorBillings), "next-page", HttpMethods.Get, new
                {
                    page = parameters.Page + 1,
                    pageSize = parameters.PageSize,
                    fields = parameters.Fields,
                    search = parameters.Search,
                    sort = parameters.Sort,
                    startDate = parameters.StartDate,
                    endDate = parameters.EndDate,
                    status = parameters.Status
                }));
            }

            if (hasPreviousPage)
            {
                links.Add(_linkService.Create(nameof(GetMyDoctorBillings), "previous-page", HttpMethods.Get, new
                {
                    page = parameters.Page - 1,
                    pageSize = parameters.PageSize,
                    fields = parameters.Fields,
                    search = parameters.Search,
                    sort = parameters.Sort,
                    startDate = parameters.StartDate,
                    endDate = parameters.EndDate,
                    status = parameters.Status
                }));
            }

            return links;
        }
    }
}