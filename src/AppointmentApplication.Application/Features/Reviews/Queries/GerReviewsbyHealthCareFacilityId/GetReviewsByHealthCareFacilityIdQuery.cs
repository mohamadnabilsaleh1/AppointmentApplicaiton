using System;
using System.Dynamic;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Reviews.Queries.GetReviewsByHealthCareFacilityId
{
    public sealed record GetReviewsByHealthCareFacilityIdQuery(
        Guid UserId,
        string? Search,
        int Page = 1,
        int PageSize = 10,
        string? Sort = "CreatedAtUtc",
        string? Fields = null,
        int? MinRating = null,
        int? MaxRating = null,
        DateTime? FromDate = null,
        DateTime? ToDate = null,
        Guid? DoctorId = null,
        Guid? PatientId = null
    ) : ICachedQuery<Result<PaginationResult<ExpandoObject>>>
    {
        public string CacheKey => $"reviews-facility-{UserId}-page-{Page}-size-{PageSize}";
        public string[] Tags => new[] { "reviews", $"facility-{UserId}" };
        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
    }
}