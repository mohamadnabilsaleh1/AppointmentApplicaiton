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

        public static Error CreateAppointmentFailed(string error) =>
            Error.Failure("Appointment.CreateFailed", $"Failed to create appointment: {error}");
    }
}