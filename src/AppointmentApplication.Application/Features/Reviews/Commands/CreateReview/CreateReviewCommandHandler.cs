using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Reviews.Dtos;
using AppointmentApplication.Application.Features.Reviews.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Appointments.Enums;
using AppointmentApplication.Domain.Reviews;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Result<ReviewDto>>
    {
        private readonly ILogger<CreateReviewCommandHandler> _logger;
        private readonly IAppDbContext _context;

        public CreateReviewCommandHandler(
            ILogger<CreateReviewCommandHandler> logger,
            IAppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result<ReviewDto>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {

            _logger.LogInformation(
                "Starting review creation for UserId: {UserId}, AppointmentId: {AppointmentId}",
                request.UserId, request.AppointmentId);

            // 1️⃣ Get patient by user ID
            var patient = await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == request.UserId && p.IsActive, cancellationToken);

            if (patient == null)
            {
                _logger.LogWarning("Review creation failed. Patient not found. UserId: {UserId}", request.UserId);
                return ApplicationReviewErrors.PatientNotFound(request.UserId);
            }

            // 2️⃣ Get appointment and check if it's completed
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Facility)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                _logger.LogWarning("Review creation failed. Appointment not found. AppointmentId: {AppointmentId}", request.AppointmentId);
                return ApplicationReviewErrors.AppointmentNotFound(request.AppointmentId);
            }

            // 3️⃣ Verify the appointment belongs to the patient
            if (appointment.PatientId != patient.Id)
            {
                _logger.LogWarning(
                    "Review creation failed. Appointment {AppointmentId} does not belong to patient {PatientId}",
                    request.AppointmentId, patient.Id);
                return ApplicationReviewErrors.AppointmentNotOwnedByPatient(request.AppointmentId, patient.Id);
            }

            // 4️⃣ Check if appointment is completed
            if (appointment.Status != AppointmentStatus.Completed)
            {
                _logger.LogWarning(
                    "Review creation failed. Appointment {AppointmentId} is not completed. Current status: {Status}",
                    request.AppointmentId, appointment.Status);
                return ApplicationReviewErrors.AppointmentNotCompleted(request.AppointmentId);
            }

            // 5️⃣ Check if review already exists for this appointment
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.AppointmentId == request.AppointmentId, cancellationToken);

            if (existingReview != null)
            {
                _logger.LogWarning(
                    "Review creation failed. Review already exists for appointment {AppointmentId}",
                    request.AppointmentId);
                return ApplicationReviewErrors.ReviewAlreadyExists(request.AppointmentId);
            }

            // 6️⃣ Create review using domain entity
            var reviewResult = Review.Create(
                patientId: patient.Id,
                facilityId: appointment.FacilityId,
                doctorId: appointment.DoctorId,
                appointmentId: appointment.Id,
                rating: request.Rating,
                comment: request.Comment);

            if (reviewResult.IsError)
            {
                _logger.LogWarning("Review creation failed: {Errors}", string.Join(", ", reviewResult.Errors));
                return ApplicationReviewErrors.CreateReviewFailed(string.Join(", ", reviewResult.Errors));
            }

            var review = reviewResult.Value;

            // 7️⃣ Save to database
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Review created successfully. ReviewId: {ReviewId}, AppointmentId: {AppointmentId}, Rating: {Rating}",
                review.Id, review.AppointmentId, review.Rating);

            // 8️⃣ Convert to DTO
            var reviewDto = MapToDto(review, appointment);

            return reviewDto;

        }

        private static ReviewDto MapToDto(Review review, Appointment appointment)
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
                PatientName: $"{appointment.Patient.FirstName} {appointment.Patient.LastName}",
                DoctorName: $"{appointment.Doctor.FirstName} {appointment.Doctor.LastName}",
                FacilityName: appointment.Facility.Name
            );
        }
    }
}