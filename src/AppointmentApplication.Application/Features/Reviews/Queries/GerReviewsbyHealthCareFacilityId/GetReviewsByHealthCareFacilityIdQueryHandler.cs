using System.Dynamic;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.Reviews.Dtos;
using AppointmentApplication.Application.Features.Reviews.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Query;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Reviews;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Reviews.Queries.GetReviewsByHealthCareFacilityId
{
    public class GetReviewsByHealthCareFacilityIdQueryHandler
        : IRequestHandler<GetReviewsByHealthCareFacilityIdQuery, Result<PaginationResult<ExpandoObject>>>
    {
        private readonly ILogger<GetReviewsByHealthCareFacilityIdQueryHandler> _logger;
        private readonly IAppDbContext _context;
        private readonly DynamicQueryService _dynamicQueryService;
        private readonly DataShapingService _dataShapingService;

        public GetReviewsByHealthCareFacilityIdQueryHandler(
            ILogger<GetReviewsByHealthCareFacilityIdQueryHandler> logger,
            IAppDbContext context,
            DataShapingService dataShapingService,
            DynamicQueryService dynamicQueryService)
        {
            _logger = logger;
            _context = context;
            _dataShapingService = dataShapingService;
            _dynamicQueryService = dynamicQueryService;
        }

        public async Task<Result<PaginationResult<ExpandoObject>>> Handle(
            GetReviewsByHealthCareFacilityIdQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving reviews for FacilityUserId: {UserId}", request.UserId);

            var facility = await _context.HealthcareFacilities
                .FirstOrDefaultAsync(f => f.UserId == request.UserId && f.IsActive, cancellationToken);

            if (facility is null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
            }

            IQueryable<Review> query = _context.Reviews
                .Include(r => r.Patient).ThenInclude(p => p.User)
                .Include(r => r.Doctor).ThenInclude(d => d.User)
                .Include(r => r.Facility)
                .Include(r => r.Appointment)
                .Where(r => r.FacilityID == facility.Id)
                .AsQueryable();

            // Dynamic filters
            var filters = new Dictionary<string, object?>
            {
                { "Rating",
                    (request.MinRating.HasValue || request.MaxRating.HasValue)
                    ? new { Min = request.MinRating, Max = request.MaxRating }
                    : null
                },
                { "DoctorID", request.DoctorId },
                { "PatientID", request.PatientId },
                { "CreatedAtUtc",
                    (request.FromDate.HasValue || request.ToDate.HasValue)
                    ? new { From = request.FromDate, To = request.ToDate }
                    : null
                }
            };

            var dynamicQueryResult =
                await _dynamicQueryService.ExecuteAsync<Review, HealthCareFacilityReviewDto>(
                    query: query,
                    searchTerm: request.Search,
                    searchProperties: new[]
                    {
                        "Comment",
                        "Patient.FirstName",
                        "Patient.LastName",
                        "Doctor.FirstName",
                        "Doctor.LastName"
                    },
                    sortBy: request.Sort ?? "CreatedAtUtc",
                    page: request.Page,
                    pageSize: request.PageSize,
                    fields: request.Fields,
                    toDtoFunc: list => list.ToHealthCareFacilityReviewDtos(),
                    filters: filters
                );

            return dynamicQueryResult;
        }
    }
}
