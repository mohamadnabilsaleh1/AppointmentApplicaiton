using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Reviews
{
    public static class ReviewErrors
    {
        public static readonly Error PatientIdRequired =
            Error.Validation("Review.PatientIdRequired", "Patient ID is required");

        public static readonly Error FacilityIdRequired =
            Error.Validation("Review.FacilityIdRequired", "Facility ID is required");

        public static readonly Error DoctorIdRequired =
            Error.Validation("Review.DoctorIdRequired", "Doctor ID is required");

        public static readonly Error AppointmentIdRequired =
            Error.Validation("Review.AppointmentIdRequired", "Appointment ID is required");

        public static readonly Error InvalidRating =
            Error.Validation("Review.InvalidRating", "Rating must be between 1 and 5");

        public static readonly Error CommentTooLong =
            Error.Validation("Review.CommentTooLong", "Comment cannot exceed 1000 characters");

        public static readonly Error ReviewAlreadyExists =
            Error.Conflict("Review.ReviewAlreadyExists", "Review already exists for this appointment");

        public static readonly Error AppointmentNotCompleted =
            Error.Validation("Review.AppointmentNotCompleted", "Cannot review an appointment that is not completed");

        public static readonly Error AppointmentNotOwnedByPatient =
            Error.Validation("Review.AppointmentNotOwnedByPatient", "Appointment does not belong to the patient");

        public static readonly Error ReviewNotFound =
            Error.NotFound("Review.ReviewNotFound", "Review not found");

        public static readonly Error CannotModifyReview =
            Error.Validation("Review.CannotModifyReview", "Cannot modify review after 24 hours of creation");

        public static readonly Error InvalidDoctor = Error.Validation(
        "Review.InvalidDoctor",
        "Review does not belong to the specified doctor.");
    }
}