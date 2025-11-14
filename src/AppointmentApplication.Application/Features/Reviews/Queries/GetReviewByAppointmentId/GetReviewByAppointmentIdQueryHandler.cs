using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Reviews.Dtos;
using AppointmentApplication.Application.Features.Reviews.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Reviews.Queries.GetReviewByAppointmentId
{
    public class GetReviewByAppointmentIdQueryHandler : IRequestHandler<GetReviewByAppointmentIdQuery, Result<ReviewDto>>
    {
        private readonly ILogger<GetReviewByAppointmentIdQueryHandler> _logger;
        private readonly IAppDbContext _context;

        public GetReviewByAppointmentIdQueryHandler(
            ILogger<GetReviewByAppointmentIdQueryHandler> logger,
            IAppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result<ReviewDto>> Handle(GetReviewByAppointmentIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching review for AppointmentId: {AppointmentId}", 
                    request.AppointmentId);

                // 1️⃣ Get review with all related data
                var review = await _context.Reviews
                    .Include(r => r.Patient)
                    .Include(r => r.Doctor)
                    .Include(r => r.Facility)
                    .Include(r => r.Appointment)
                    .FirstOrDefaultAsync(r => r.AppointmentId == request.AppointmentId, cancellationToken);

                // 2️⃣ Check if review exists
                if (review == null)
                {
                    _logger.LogWarning("Review not found for AppointmentId: {AppointmentId}", request.AppointmentId);
                    return ApplicationReviewErrors.ReviewNotFound(request.AppointmentId);
                }

                // 3️⃣ Convert to DTO
                var reviewDto = MapToDto(review);

                _logger.LogInformation(
                    "Successfully retrieved review {ReviewId} for AppointmentId: {AppointmentId}", 
                    review.Id, request.AppointmentId);

                return reviewDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "Error fetching review for AppointmentId: {AppointmentId}", 
                    request.AppointmentId);
                return ApplicationReviewErrors.CreateReviewFailed($"Error fetching review: {ex.Message}");
            }
        }

        private static ReviewDto MapToDto(Domain.Reviews.Review review)
        {
            return new ReviewDto(
                Id: review.Id,
                PatientId: review.PatientID,
                DoctorId: review.DoctorID,
                FacilityId: review.FacilityID,
                AppointmentId: review.AppointmentId,
                Rating: review.Rating,
                Comment: review.Comment,
                CreatedAt: review.CreatedAtUtc,
                PatientName: $"{review.Patient?.FirstName} {review.Patient?.LastName}",
                DoctorName: $"{review.Doctor?.FirstName} {review.Doctor?.LastName}",
                FacilityName: review.Facility?.Name ?? "Unknown Facility"
            );
        }
    }
}