// using AppointmentApplication.API.Dtos;
// using AppointmentApplication.API.Services;
// using AppointmentApplication.Application.Abstractions.Authentication;
// using AppointmentApplication.Application.Features.Reviews.Commands.CreateReview;

// using AppointmentApplication.Application.Features.Reviews.Queries.GetReviewByAppointmentId;
// using AppointmentApplication.Application.Features.Reviews.Queries.GetReviewsByHealthCareFacilityId;
// using AppointmentApplication.Application.Shared.Services;

// using Asp.Versioning;

// using MediatR;

// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.OutputCaching;
// using AppointmentApplication.API.Dtos.Reviews;
// using System.Dynamic;
// using AppointmentApplication.Application.Features.Reviews.Dtos;

// namespace AppointmentApplication.API.Controllers;

// [Route("api/reviews")]
// public sealed class ReviewController : ApiController
// {
//     private readonly ISender _sender;
//     private readonly LinkService _linkService;
//     private readonly IUserContext _userContext;

//     public ReviewController(ISender sender, LinkService linkService, IUserContext userContext)
//     {
//         _sender = sender;
//         _linkService = linkService;
//         _userContext = userContext;
//     }

//     // [HttpPost]
//     // [Authorize(Roles = $"{Roles.Patient}")]
//     // [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status201Created)]
//     // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
//     // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
//     // [MapToApiVersion("0.1")]
//     // [EndpointSummary("Creates a new Review.")]
//     // [EndpointDescription("Adds a new Review for a completed appointment.")]
//     // [EndpointName("CreateReview")]
//     // public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request, CancellationToken cancellationToken)
//     // {
//     //     var result = await _sender.Send(
//     //         new CreateReviewCommand(
//     //             _userContext.UserId,
//     //             request.AppointmentId,
//     //             request.Rating,
//     //             request.Comment),
//     //         cancellationToken);

//     //     return result.Match(
//     //         response =>
//     //         {
//     //             var links = CreateLinksForReview(response.Id.ToString(), response.AppointmentId.ToString());

//     //             var resource = new
//     //             {
//     //                 data = response,
//     //                 links
//     //             };

//     //             return CreatedAtRoute(
//     //                 routeName: "GetReviewByAppointmentId",
//     //                 routeValues: new { appointmentId = response.AppointmentId, apiVersion = "0.1" },
//     //                 value: resource);
//     //         },
//     //         Problem);
//     // }

//     [HttpGet("appointment/{appointmentId:guid}", Name = "GetReviewByAppointmentId")]
//     [Authorize]
//     [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
//     [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
//     [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
//     [MapToApiVersion("0.1")]
//     [EndpointSummary("Get Review by Appointment Id.")]
//     [EndpointDescription("Retrieves a review for a specific appointment.")]
//     [EndpointName("GetReviewByAppointmentId")]
//     public async Task<IActionResult> GetReviewByAppointmentId(Guid appointmentId, CancellationToken cancellationToken)
//     {
//         var result = await _sender.Send(new GetReviewByAppointmentIdQuery(appointmentId), cancellationToken);

//         return result.Match(
//             review =>
//             {
//                 var links = CreateLinksForReview(review.Id.ToString(), review.AppointmentId.ToString());
//                 var resource = new
//                 {
//                     data = review,
//                     links
//                 };
//                 return Ok(resource);
//             },
//             Problem);
//     }

//     [HttpGet("facility/{facilityId:guid}")]
//     [AllowAnonymous]
//     [ProducesResponseType(typeof(PaginationResult<ExpandoObject>), StatusCodes.Status200OK)]
//     [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
//     [OutputCache(Duration = 60)]
//     [MapToApiVersion("0.1")]
//     [EndpointSummary("Get Reviews by Healthcare Facility Id.")]
//     [EndpointDescription("Retrieves reviews for a healthcare facility with optional filtering and pagination.")]
//     [EndpointName("GetReviewsByHealthCareFacilityId")]
//     public async Task<IActionResult> GetReviewsByHealthCareFacilityId(
//         Guid facilityId,
//         [FromQuery] ReviewQueryParameters queryParameters,
//         CancellationToken cancellationToken)
//     {
//         var result = await _sender.Send(
//             new GetReviewsByHealthCareFacilityIdQuery(
//                 facilityId,
//                 queryParameters.Search,
//                 queryParameters.Page,
//                 queryParameters.PageSize,
//                 queryParameters.Sort,
//                 queryParameters.Fields,
//                 queryParameters.MinRating,
//                 queryParameters.MaxRating,
//                 queryParameters.FromDate,
//                 queryParameters.ToDate,
//                 queryParameters.DoctorId,
//                 queryParameters.PatientId),
//             cancellationToken);

//         return result.Match(
//             response =>
//             {
//                 var hasNextPage = response.Page < response.TotalPages;
//                 var hasPreviousPage = response.Page > 1;

//                 var links = CreateLinksForReviews(facilityId, queryParameters, hasNextPage, hasPreviousPage);

//                 var resource = new
//                 {
//                     data = response.Items,
//                     pagination = new
//                     {
//                         response.Page,
//                         response.PageSize,
//                         response.TotalCount,
//                         response.TotalPages
//                     },
//                     links
//                 };

//                 return Ok(resource);
//             },
//             Problem);
//     }

//     // [HttpGet("doctor/{doctorId:guid}")]
//     // [AllowAnonymous]
//     // [ProducesResponseType(typeof(PaginationResult<ExpandoObject>), StatusCodes.Status200OK)]
//     // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
//     // [OutputCache(Duration = 60)]
//     // [MapToApiVersion("0.1")]
//     // [EndpointSummary("Get Reviews by Doctor Id.")]
//     // [EndpointDescription("Retrieves reviews for a specific doctor with optional filtering and pagination.")]
//     // [EndpointName("GetReviewsByDoctorId")]
//     // public async Task<IActionResult> GetReviewsByDoctorId(
//     //     Guid doctorId,
//     //     [FromQuery] ReviewQueryParameters queryParameters,
//     //     CancellationToken cancellationToken)
//     // {
//     //     var result = await _sender.Send(
//     //         new GetReviewsByDoctorIdQuery(
//     //             doctorId,
//     //             queryParameters.Search,
//     //             queryParameters.Page,
//     //             queryParameters.PageSize,
//     //             queryParameters.Sort,
//     //             queryParameters.Fields,
//     //             queryParameters.MinRating,
//     //             queryParameters.MaxRating,
//     //             queryParameters.FromDate,
//     //             queryParameters.ToDate),
//     //         cancellationToken);

//     //     return result.Match(
//     //         response =>
//     //         {
//     //             var hasNextPage = response.Page < response.TotalPages;
//     //             var hasPreviousPage = response.Page > 1;

//     //             var links = CreateLinksForDoctorReviews(doctorId, queryParameters, hasNextPage, hasPreviousPage);

//     //             var resource = new
//     //             {
//     //                 data = response.Items,
//     //                 pagination = new
//     //                 {
//     //                     response.Page,
//     //                     response.PageSize,
//     //                     response.TotalCount,
//     //                     response.TotalPages
//     //                 },
//     //                 links
//     //             };

//     //             return Ok(resource);
//     //         },
//     //         Problem);
//     // }

//     // [HttpPut("{reviewId:guid}")]
//     // [Authorize(Roles = $"{Roles.Patient}")]
//     // [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
//     // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
//     // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
//     // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
//     // [MapToApiVersion("0.1")]
//     // [EndpointSummary("Update a Review.")]
//     // [EndpointDescription("Updates an existing review (within 24 hours of creation).")]
//     // [EndpointName("UpdateReview")]
//     // public async Task<IActionResult> UpdateReview(
//     //     Guid reviewId,
//     //     [FromBody] UpdateReviewRequest request,
//     //     CancellationToken cancellationToken)
//     // {
//     //     var result = await _sender.Send(
//     //         new UpdateReviewCommand(
//     //             reviewId,
//     //             request.Rating,
//     //             request.Comment),
//     //         cancellationToken);

//     //     return result.Match(
//     //         response =>
//     //         {
//     //             var links = CreateLinksForReview(response.Id.ToString(), response.AppointmentId.ToString());
//     //             var resource = new
//     //             {
//     //                 data = response,
//     //                 links
//     //             };
//     //             return Ok(resource);
//     //         },
//     //         Problem);
//     // }

//     // [HttpDelete("{reviewId:guid}")]
//     // [Authorize(Roles = $"{Roles.Patient},{Roles.Admin}")]
//     // [ProducesResponseType(StatusCodes.Status204NoContent)]
//     // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
//     // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
//     // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
//     // [MapToApiVersion("0.1")]
//     // [EndpointSummary("Delete a Review.")]
//     // [EndpointDescription("Deletes an existing review.")]
//     // [EndpointName("DeleteReview")]
//     // public async Task<IActionResult> DeleteReview(Guid reviewId, CancellationToken cancellationToken)
//     // {
//     //     var result = await _sender.Send(
//     //         new DeleteReviewCommand(reviewId, _userContext.UserId),
//     //         cancellationToken);

//     //     return result.Match(
//     //         _ => NoContent(),
//     //         Problem);
//     // }

//     [HttpGet("my-reviews")]
//     [Authorize(Roles = $"{Roles.Patient}")]
//     [ProducesResponseType(typeof(PaginationResult<ExpandoObject>), StatusCodes.Status200OK)]
//     [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
//     [MapToApiVersion("0.1")]
//     [EndpointSummary("Get current user's reviews.")]
//     [EndpointDescription("Retrieves reviews written by the currently authenticated patient.")]
//     [EndpointName("GetMyReviews")]
//     public async Task<IActionResult> GetMyReviews(
//         [FromQuery] ReviewQueryParameters queryParameters,
//         CancellationToken cancellationToken)
//     {
//         var result = await _sender.Send(
//             new GetReviewsByHealthCareFacilityIdQuery(
//                 Guid.Empty, // Facility ID not needed for patient's reviews
//                 queryParameters.Search,
//                 queryParameters.Page,
//                 queryParameters.PageSize,
//                 queryParameters.Sort,
//                 queryParameters.Fields,
//                 queryParameters.MinRating,
//                 queryParameters.MaxRating,
//                 queryParameters.FromDate,
//                 queryParameters.ToDate,
//                 null, // Doctor ID filter
//                 _userContext.UserId), // Filter by current patient
//             cancellationToken);

//         return result.Match(
//             response =>
//             {
//                 var hasNextPage = response.Page < response.TotalPages;
//                 var hasPreviousPage = response.Page > 1;

//                 var links = CreateLinksForMyReviews(queryParameters, hasNextPage, hasPreviousPage);

//                 var resource = new
//                 {
//                     data = response.Items,
//                     pagination = new
//                     {
//                         response.Page,
//                         response.PageSize,
//                         response.TotalCount,
//                         response.TotalPages
//                     },
//                     links
//                 };

//                 return Ok(resource);
//             },
//             Problem);
//     }

//     private List<LinkDto> CreateLinksForReview(string reviewId, string appointmentId)
//     {
//         var links = new List<LinkDto>
//         {
//             _linkService.Create(nameof(GetReviewByAppointmentId), "self", HttpMethods.Get, new { appointmentId }),
//             // _linkService.Create(nameof(UpdateReview), "update", HttpMethods.Put, new { reviewId }),
//             // _linkService.Create(nameof(DeleteReview), "delete", HttpMethods.Delete, new { reviewId }),
//             _linkService.Create(nameof(GetMyReviews), "my-reviews", HttpMethods.Get),
//             _linkService.Create(nameof(CreateReview), "create", HttpMethods.Post)
//         };

//         return links;
//     }

//     private List<LinkDto> CreateLinksForReviews(Guid facilityId, ReviewQueryParameters parameters, bool hasNextPage, bool hasPreviousPage)
//     {
//         var links = new List<LinkDto>
//         {
//             _linkService.Create(nameof(GetReviewsByHealthCareFacilityId), "self", HttpMethods.Get, new
//             {
//                 facilityId,
//                 page = parameters.Page,
//                 pageSize = parameters.PageSize,
//                 fields = parameters.Fields,
//                 search = parameters.Search,
//                 sort = parameters.Sort,
//                 minRating = parameters.MinRating,
//                 maxRating = parameters.MaxRating
//             })
//         };

//         if (hasNextPage)
//         {
//             links.Add(_linkService.Create(nameof(GetReviewsByHealthCareFacilityId), "next-page", HttpMethods.Get, new
//             {
//                 facilityId,
//                 page = parameters.Page + 1,
//                 pageSize = parameters.PageSize,
//                 fields = parameters.Fields,
//                 search = parameters.Search,
//                 sort = parameters.Sort,
//                 minRating = parameters.MinRating,
//                 maxRating = parameters.MaxRating
//             }));
//         }

//         if (hasPreviousPage)
//         {
//             links.Add(_linkService.Create(nameof(GetReviewsByHealthCareFacilityId), "previous-page", HttpMethods.Get, new
//             {
//                 facilityId,
//                 page = parameters.Page - 1,
//                 pageSize = parameters.PageSize,
//                 fields = parameters.Fields,
//                 search = parameters.Search,
//                 sort = parameters.Sort,
//                 minRating = parameters.MinRating,
//                 maxRating = parameters.MaxRating
//             }));
//         }

//         links.Add(_linkService.Create(nameof(CreateReview), "create", HttpMethods.Post));

//         return links;
//     }

//     // private List<LinkDto> CreateLinksForDoctorReviews(Guid doctorId, ReviewQueryParameters parameters, bool hasNextPage, bool hasPreviousPage)
//     // {
//     //     var links = new List<LinkDto>
//     //     {
//     //         _linkService.Create(nameof(GetReviewsByDoctorId), "self", HttpMethods.Get, new
//     //         {
//     //             doctorId,
//     //             page = parameters.Page,
//     //             pageSize = parameters.PageSize,
//     //             fields = parameters.Fields,
//     //             search = parameters.Search,
//     //             sort = parameters.Sort,
//     //             minRating = parameters.MinRating,
//     //             maxRating = parameters.MaxRating
//     //         })
//     //     };

//     //     if (hasNextPage)
//     //     {
//     //         links.Add(_linkService.Create(nameof(GetReviewsByDoctorId), "next-page", HttpMethods.Get, new
//     //         {
//     //             doctorId,
//     //             page = parameters.Page + 1,
//     //             pageSize = parameters.PageSize,
//     //             fields = parameters.Fields,
//     //             search = parameters.Search,
//     //             sort = parameters.Sort,
//     //             minRating = parameters.MinRating,
//     //             maxRating = parameters.MaxRating
//     //         }));
//     //     }

//     //     if (hasPreviousPage)
//     //     {
//     //         links.Add(_linkService.Create(nameof(GetReviewsByDoctorId), "previous-page", HttpMethods.Get, new
//     //         {
//     //             doctorId,
//     //             page = parameters.Page - 1,
//     //             pageSize = parameters.PageSize,
//     //             fields = parameters.Fields,
//     //             search = parameters.Search,
//     //             sort = parameters.Sort,
//     //             minRating = parameters.MinRating,
//     //             maxRating = parameters.MaxRating
//     //         }));
//     //     }

//     //     return links;
//     // }

//     private List<LinkDto> CreateLinksForMyReviews(ReviewQueryParameters parameters, bool hasNextPage, bool hasPreviousPage)
//     {
//         var links = new List<LinkDto>
//         {
//             _linkService.Create(nameof(GetMyReviews), "self", HttpMethods.Get, new
//             {
//                 page = parameters.Page,
//                 pageSize = parameters.PageSize,
//                 fields = parameters.Fields,
//                 search = parameters.Search,
//                 sort = parameters.Sort,
//                 minRating = parameters.MinRating,
//                 maxRating = parameters.MaxRating
//             })
//         };

//         if (hasNextPage)
//         {
//             links.Add(_linkService.Create(nameof(GetMyReviews), "next-page", HttpMethods.Get, new
//             {
//                 page = parameters.Page + 1,
//                 pageSize = parameters.PageSize,
//                 fields = parameters.Fields,
//                 search = parameters.Search,
//                 sort = parameters.Sort,
//                 minRating = parameters.MinRating,
//                 maxRating = parameters.MaxRating
//             }));
//         }

//         if (hasPreviousPage)
//         {
//             links.Add(_linkService.Create(nameof(GetMyReviews), "previous-page", HttpMethods.Get, new
//             {
//                 page = parameters.Page - 1,
//                 pageSize = parameters.PageSize,
//                 fields = parameters.Fields,
//                 search = parameters.Search,
//                 sort = parameters.Sort,
//                 minRating = parameters.MinRating,
//                 maxRating = parameters.MaxRating
//             }));
//         }

//         links.Add(_linkService.Create(nameof(CreateReview), "create", HttpMethods.Post));

//         return links;
//     }
// }