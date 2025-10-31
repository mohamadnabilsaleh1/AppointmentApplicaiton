// Domain/Appointments/Errors/AppointmentErrors.cs
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Appointments.Errors
{
    public static class AppointmentErrors
    {
        public static readonly Error NotFound =
            Error.NotFound("Appointment.NotFound", "Appointment not found.");

        public static readonly Error InvalidPatientId =
            Error.Validation("Appointment.InvalidPatientId", "Patient ID cannot be empty.");

        public static readonly Error InvalidDoctorId =
            Error.Validation("Appointment.InvalidDoctorId", "Doctor ID cannot be empty.");

        public static readonly Error InvalidFacilityId =
            Error.Validation("Appointment.InvalidFacilityId", "Facility ID cannot be empty.");

        public static readonly Error InvalidScheduledDate =
            Error.Validation("Appointment.InvalidScheduledDate", "Scheduled date cannot be in the past.");

        public static readonly Error InvalidDuration =
            Error.Validation(
                "Appointment.InvalidDuration",
                "Duration must be between 15 minutes and 8 hours.");

        public static readonly Error InvalidScheduledTime =
            Error.Validation(
                "Appointment.InvalidScheduledTime",
                "Appointments can only be scheduled between 8 AM and 8 PM.");

        public static readonly Error EmptyNotes =
            Error.Validation("Appointment.EmptyNotes", "Notes cannot be null or empty.");

        public static readonly Error NotesTooLong =
            Error.Validation(
                "Appointment.NotesTooLong",
                "Notes cannot exceed 1000 characters.");

        // Status transition errors
        public static readonly Error InvalidStatusTransition =
            Error.Conflict(
                "Appointment.InvalidStatusTransition",
                "Invalid appointment status transition.");

        public static readonly Error CannotConfirmCompleted =
            Error.Conflict(
                "Appointment.CannotConfirmCompleted",
                "Cannot confirm a completed appointment.");

        public static readonly Error CannotCompleteWithoutConfirmation =
            Error.Conflict(
                "Appointment.CannotCompleteWithoutConfirmation",
                "Cannot complete an appointment that hasn't been confirmed.");

        public static readonly Error CannotCompleteWithoutPayment =
            Error.Conflict(
                "Appointment.CannotCompleteWithoutPayment",
                "Cannot complete appointment without paid billing.");

        public static readonly Error CannotCancelCompleted =
            Error.Conflict(
                "Appointment.CannotCancelCompleted",
                "Cannot cancel a completed appointment.");

        public static readonly Error EmptyCancellationReason =
            Error.Validation(
                "Appointment.EmptyCancellationReason",
                "Cancellation reason is required.");

        public static readonly Error CannotRescheduleCompleted =
            Error.Conflict(
                "Appointment.CannotRescheduleCompleted",
                "Cannot reschedule a completed appointment.");

        public static readonly Error AppointmentConflict =
            Error.Conflict(
                "Appointment.Conflict",
                "Appointment time conflicts with an existing appointment.");
    }
}