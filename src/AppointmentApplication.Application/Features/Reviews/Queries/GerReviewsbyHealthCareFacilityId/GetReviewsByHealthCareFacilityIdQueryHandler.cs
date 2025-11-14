using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppointmentApplication.Application.Features.Reviews.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Query;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Reviews;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

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

            _logger.LogInformation(
                "Fetching reviews for FacilityId: {FacilityId}, Page: {Page}, PageSize: {PageSize}",
                request.UserId, request.Page, request.PageSize);

            // 1️⃣ Validate facility exists
            var facilityExists = await _context.HealthcareFacilities
                .FirstOrDefaultAsync(f => f.UserId == request.UserId && f.IsActive, cancellationToken);

            if (facilityExists is null)
            {
                _logger.LogWarning("Facility not found or inactive. FacilityId: {FacilityId}", request.UserId);
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
            }

            // 2️⃣ Build base query with all includes
            IQueryable<Review> query = _context.Reviews
                .Include(r => r.Patient)
                    .ThenInclude(p => p.User)
                .Include(r => r.Doctor)
                    .ThenInclude(d => d.User)
                .Include(r => r.Facility)
                .Include(r => r.Appointment)
                .Where(r => r.FacilityID == facilityExists.Id)
                .AsQueryable();

            // 3️⃣ Apply dynamic filters
            var filters = new Dictionary<string, object?>
                {
                    { "Rating", request.MinRating.HasValue || request.MaxRating.HasValue
                        ? new { Min = request.MinRating, Max = request.MaxRating }
                        : null },
                    { "DoctorID", request.DoctorId },
                    { "PatientID", request.PatientId },
                    { "CreatedAt", request.FromDate.HasValue || request.ToDate.HasValue
                        ? new { From = request.FromDate, To = request.ToDate }
                        : null }
                };

            // 4️⃣ Execute dynamic query service
            var dynamicQueryResult = await _dynamicQueryService.ExecuteAsync<Review, ExpandoObject>(
                query: query,
                searchTerm: request.Search,
                searchProperties: new[] { "Comment", "Patient.FirstName", "Patient.LastName", "Doctor.FirstName", "Doctor.LastName" },
                sortBy: request.Sort,
                page: request.Page,
                pageSize: request.PageSize,
                fields: request.Fields,
                toDtoFunc: list => list.Select(review => CreateReviewExpandoObject(review)).ToList(),
                filters: filters);

            _logger.LogInformation(
                "Successfully retrieved {Count} reviews for FacilityId: {FacilityId}",
                dynamicQueryResult.Items.Count, request.UserId);
            Console.WriteLine("dynamicQueryResult=============++++>",dynamicQueryResult.Items);
            return dynamicQueryResult;

        }

        private static ExpandoObject CreateReviewExpandoObject(Review review)
        {
            dynamic expando = new ExpandoObject();
            var expandoDict = expando as IDictionary<string, object>;

            // Basic review information
            expandoDict["Id"] = review.Id;
            expandoDict["Rating"] = review.Rating;
            expandoDict["Comment"] = review.Comment;
            expandoDict["CreatedAt"] = review.CreatedAtUtc;
            expandoDict["UpdatedAt"] = review.UpdatedAtUtc;
            expandoDict["AppointmentId"] = review.AppointmentId;

            // Patient information
            expandoDict["PatientId"] = review.PatientID;
            expandoDict["PatientFirstName"] = review.Patient?.FirstName ?? "Unknown";
            expandoDict["PatientLastName"] = review.Patient?.LastName ?? "Patient";

            expandoDict["PatientFullName"] = $"{review.Patient?.FirstName} {review.Patient?.LastName}";

            // Doctor information
            expandoDict["DoctorId"] = review.DoctorID;
            expandoDict["DoctorFirstName"] = review.Doctor?.FirstName ?? "Unknown";
            expandoDict["DoctorLastName"] = review.Doctor?.LastName ?? "Doctor";
            expandoDict["DoctorSpecialization"] = review.Doctor?.Specialization;

            expandoDict["DoctorFullName"] = $"{review.Doctor?.FirstName} {review.Doctor?.LastName}";

            // Facility information
            expandoDict["FacilityId"] = review.FacilityID;
            expandoDict["FacilityName"] = review.Facility?.Name ?? "Unknown Facility";

            // Appointment information (if needed)
            if (review.Appointment != null)
            {
                expandoDict["AppointmentDate"] = review.Appointment.ScheduledDate;
                expandoDict["AppointmentTime"] = review.Appointment.ScheduledTime;
            }

            return expando;
        }
    }
}