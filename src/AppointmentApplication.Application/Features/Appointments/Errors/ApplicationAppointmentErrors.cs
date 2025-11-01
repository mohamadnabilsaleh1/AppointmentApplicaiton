using AppointmentApplication.Domain.Appointments.Enums;
using AppointmentApplication.Domain.Shared.Results;

using System;

namespace AppointmentApplication.Application.Features.Appointments.Errors
{
    public static class ApplicationAppointmentErrors
    {
        // Validation Errors
        public static readonly Error InvalidPatientId =
            Error.Validation("Appointment.InvalidPatientId", "Patient ID cannot be empty.");

        public static readonly Error InvalidDoctorId =
            Error.Validation("Appointment.InvalidDoctorId", "Doctor ID cannot be empty.");

        public static readonly Error InvalidFacilityId =
            Error.Validation("Appointment.InvalidFacilityId", "Facility ID cannot be empty.");

        public static readonly Error InvalidScheduledDate =
            Error.Validation("Appointment.InvalidScheduledDate", "Scheduled date cannot be in the past.");

        public static readonly Error InvalidScheduledDateFuture =
            Error.Validation("Appointment.InvalidScheduledDateFuture", "Scheduled date cannot be more than 1 year in the future.");

        public static readonly Error InvalidScheduledTime =
            Error.Validation("Appointment.InvalidScheduledTime", "Appointments must be scheduled between 8 AM and 8 PM.");

        public static readonly Error InvalidDuration =
            Error.Validation("Appointment.InvalidDuration", "Duration must be between 15 minutes and 8 hours.");

        public static readonly Error EmptyNotes =
            Error.Validation("Appointment.EmptyNotes", "Notes are required.");

        public static readonly Error NotesTooLong =
            Error.Validation("Appointment.NotesTooLong", "Notes cannot exceed 1000 characters.");

        public static readonly Error InvalidTotalAmount =
            Error.Validation("Appointment.InvalidTotalAmount", "Total amount must be greater than 0.");

        // Business Rule Errors
        public static Error PatientNotFound(Guid patientId) =>
            Error.NotFound("Appointment.PatientNotFound", $"Patient with ID {patientId} not found or inactive.");

        public static Error DoctorNotFound(Guid doctorId) =>
            Error.NotFound("Appointment.DoctorNotFound", $"Doctor with ID {doctorId} not found or inactive.");

        public static Error FacilityNotFound(Guid facilityId) =>
            Error.NotFound("Appointment.FacilityNotFound", $"Facility with ID {facilityId} not found or inactive.");

        public static Error DoctorNotInFacility(Guid doctorId, Guid facilityId) =>
            Error.Conflict(
                "Appointment.DoctorNotInFacility",
                $"Doctor {doctorId} is not associated with facility {facilityId}.");

        public static Error DoctorNotAvailable(Guid doctorId, DateOnly date, TimeSpan time) =>
            Error.Conflict(
                "Appointment.DoctorNotAvailable",
                $"Doctor {doctorId} is not available on {date} at {time}.");

        public static Error DoctorHasException(Guid doctorId, DateOnly date) =>
            Error.Conflict(
                "Appointment.DoctorHasException",
                $"Doctor {doctorId} has a schedule exception on {date}.");

        public static Error AppointmentConflict(Guid doctorId, DateOnly date, TimeSpan time) =>
            Error.Conflict(
                "Appointment.Conflict",
                $"Appointment conflict for doctor {doctorId} on {date} at {time}.");

        // FIXED: Added proper error type for UnauthorizedToConfirmAppointment
        public static Error UnauthorizedToConfirmAppointment(Guid appointmentId) =>
            Error.Unauthorized(
                "Appointment.Confirm.Unauthorized",
                $"User is not authorized to confirm appointment {appointmentId}. Only the assigned doctor can confirm appointments.");

        // Additional errors for appointment confirmation
        public static Error CannotConfirmAppointment(AppointmentStatus currentStatus) =>
            Error.Conflict(
                "Appointment.Confirm.InvalidStatus",
                $"Cannot confirm appointment with current status: {currentStatus}. Only appointments with 'Pending' status can be confirmed.");

        public static Error CannotConfirmPastAppointment(DateOnly scheduledDate) =>
            Error.Conflict(
                "Appointment.Confirm.PastDate",
                $"Cannot confirm appointment scheduled for {scheduledDate}. Past appointments cannot be confirmed.");

        public static Error AppointmentNotFound(Guid appointmentId) =>
            Error.NotFound(
                "Appointment.NotFound",
                $"Appointment with ID {appointmentId} was not found.");

        // Additional common appointment errors
        public static Error InvalidStatusTransition =>
            Error.Conflict(
                "Appointment.InvalidStatusTransition",
                "Invalid appointment status transition.");

        public static Error CannotCompleteWithoutConfirmation =>
            Error.Conflict(
                "Appointment.CannotCompleteWithoutConfirmation",
                "Cannot complete appointment without confirmation.");

        public static Error CannotCompleteWithoutPayment =>
            Error.Conflict(
                "Appointment.CannotCompleteWithoutPayment",
                "Cannot complete appointment without payment.");

        public static Error CannotCancelCompleted =>
            Error.Conflict(
                "Appointment.CannotCancelCompleted",
                "Cannot cancel completed or already cancelled appointment.");

        public static Error EmptyCancellationReason =>
            Error.Validation(
                "Appointment.EmptyCancellationReason",
                "Cancellation reason is required.");

        public static Error CannotRescheduleCompleted =>
            Error.Conflict(
                "Appointment.CannotRescheduleCompleted",
                "Cannot reschedule completed or cancelled appointment.");

        public static Error UnauthorizedToCancelAppointment(Guid appointmentId) =>
        Error.Unauthorized(
            "Appointment.Cancel.Unauthorized",
            $"User is not authorized to cancel appointment {appointmentId}. " +
            "Only the assigned doctor, patient who booked it, or admin can cancel appointments.");

        public static Error AppointmentAlreadyCancelled(Guid appointmentId) =>
            Error.Conflict(
                "Appointment.Cancel.AlreadyCancelled",
                $"Appointment {appointmentId} is already cancelled.");

        public static Error AppointmentAlreadyConfirmed(Guid appointmentId) =>
    Error.Conflict(
        "Appointment.Cancel.AlreadyCancelled",
        $"Appointment {appointmentId} is already cancelled.");

        public static Error CannotCancelCompletedAppointment(Guid appointmentId) =>
            Error.Conflict(
                "Appointment.Cancel.Completed",
                $"Cannot cancel appointment {appointmentId} because it's already completed.");

        public static Error CannotCancelWithin24Hours(Guid appointmentId) =>
            Error.Conflict(
                "Appointment.Cancel.Within24Hours",
                $"Appointment {appointmentId} cannot be cancelled within 24 hours of the scheduled time.");

        public static Error CannotCancelPastAppointment(DateOnly scheduledDate) =>
            Error.Conflict(
                "Appointment.Cancel.PastDate",
                $"Cannot cancel appointment scheduled for {scheduledDate}. Past appointments cannot be cancelled.");

        public static Error CancellationReasonTooLong =>
            Error.Validation(
                "Appointment.Cancel.ReasonTooLong",
                "Cancellation reason cannot exceed 500 characters.");

                        
        public static Error UnauthorizedToCompleteAppointment(Guid appointmentId) =>
            Error.Unauthorized(
                "Appointment.Complete.Unauthorized",
                $"User is not authorized to complete appointment {appointmentId}. Only the assigned doctor can complete appointments.");

        public static Error CannotCompleteAppointment(AppointmentStatus currentStatus) =>
            Error.Conflict(
                "Appointment.Complete.InvalidStatus",
                $"Cannot complete appointment with current status: {currentStatus}. Only appointments with 'Confirmed' status can be completed.");
    }
}