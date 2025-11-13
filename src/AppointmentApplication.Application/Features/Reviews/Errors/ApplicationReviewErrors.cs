using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Application.Features.Reviews.Errors
{
    public class ApplicationReviewErrors
    {
        public static Error PatientNotFound(Guid userId) =>
            Error.NotFound(
                "Review.PatientNotFound",
                $"Patient with user ID '{userId}' was not found.");

        public static Error AppointmentNotFound(Guid appointmentId) =>
            Error.NotFound(
                "Review.AppointmentNotFound",
                $"Appointment with ID '{appointmentId}' was not found.");

        public static Error AppointmentNotOwnedByPatient(Guid appointmentId, Guid patientId) =>
            Error.Validation(
                "Review.AppointmentNotOwned",
                $"Appointment '{appointmentId}' does not belong to patient '{patientId}'.");

        public static Error AppointmentNotCompleted(Guid appointmentId) =>
            Error.Validation(
                "Review.AppointmentNotCompleted",
                $"Appointment '{appointmentId}' is not completed. Only completed appointments can be reviewed.");

        public static Error ReviewAlreadyExists(Guid appointmentId) =>
            Error.Conflict(
                "Review.AlreadyExists",
                $"Review already exists for appointment '{appointmentId}'.");

        public static Error ReviewNotFound(Guid reviewId) =>
            Error.NotFound(
                "Review.NotFound",
                $"Review with ID '{reviewId}' was not found.");

        public static Error CreateReviewFailed(string details) =>
            Error.Failure(
                "Review.CreateFailed",
                $"Failed to create review: {details}");

        public static Error UpdateReviewFailed(string details) =>
            Error.Failure(
                "Review.UpdateFailed",
                $"Failed to update review: {details}");

        public static Error DeleteReviewFailed(string details) =>
            Error.Failure(
                "Review.DeleteFailed",
                $"Failed to delete review: {details}");

        public static Error DatabaseSaveFailed(string errorMessage) =>
            Error.Failure(
                "Review.DatabaseSaveFailed",
                $"Failed to save review to database: {errorMessage}");

        public static Error InvalidReviewOperation(string reason) =>
            Error.Validation(
                "Review.InvalidOperation",
                $"Invalid review operation: {reason}");

        public static Error CannotModifyReviewAfter24Hours(Guid reviewId) =>
            Error.Validation(
                "Review.ModificationTimeExpired",
                $"Review '{reviewId}' cannot be modified after 24 hours of creation.");

        public static Error UserNotAuthorized(Guid reviewId, Guid userId) =>
            Error.Validation(
                "Review.UserNotAuthorized",
                $"User '{userId}' is not authorized to modify review '{reviewId}'.");
    }
}