using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Prescriptions.Errors
{
    public static class PrescriptionErrors
    {
        public static readonly Error NotFound =
            Error.NotFound("Prescription.NotFound", "Prescription not found.");

        public static readonly Error InvalidAppointmentId =
            Error.Validation("Prescription.InvalidAppointmentId", "Appointment ID cannot be empty.");

        public static readonly Error InvalidMedicationName =
            Error.Validation("Prescription.InvalidMedicationName", "Medication name cannot be empty.");

        public static readonly Error MedicationNameTooLong =
            Error.Validation(
                "Prescription.MedicationNameTooLong",
                "Medication name cannot exceed 100 characters.");

        public static readonly Error InvalidDosage =
            Error.Validation("Prescription.InvalidDosage", "Dosage cannot be empty.");

        public static readonly Error DosageTooLong =
            Error.Validation(
                "Prescription.DosageTooLong",
                "Dosage cannot exceed 50 characters.");

        public static readonly Error InvalidFrequency =
            Error.Validation("Prescription.InvalidFrequency", "Frequency cannot be empty.");

        public static readonly Error FrequencyTooLong =
            Error.Validation(
                "Prescription.FrequencyTooLong",
                "Frequency cannot exceed 50 characters.");

        public static readonly Error InvalidDuration =
            Error.Validation("Prescription.InvalidDuration", "Duration cannot be empty.");

        public static readonly Error DurationTooLong =
            Error.Validation(
                "Prescription.DurationTooLong",
                "Duration cannot exceed 50 characters.");

        public static readonly Error InvalidInstructions =
            Error.Validation("Prescription.InvalidInstructions", "Instructions cannot be empty.");

        public static readonly Error InstructionsTooLong =
            Error.Validation(
                "Prescription.InstructionsTooLong",
                "Instructions cannot exceed 500 characters.");

        public static readonly Error AlreadyDispensed =
            Error.Conflict(
                "Prescription.AlreadyDispensed",
                "Cannot modify a prescription that has been dispensed.");

        public static readonly Error AlreadyExpired =
            Error.Conflict(
                "Prescription.AlreadyExpired",
                "Cannot modify an expired prescription.");

        public static readonly Error InvalidPrescriptionDate =
            Error.Validation(
                "Prescription.InvalidPrescriptionDate",
                "Prescription date cannot be in the future.");

        public static readonly Error ExpirationDateBeforePrescriptionDate =
            Error.Validation(
                "Prescription.ExpirationDateBeforePrescriptionDate",
                "Expiration date cannot be before prescription date.");

        public static readonly Error RefillsExceeded =
            Error.Conflict(
                "Prescription.RefillsExceeded",
                "No refills remaining for this prescription.");

        public static readonly Error CannotDispenseCancelled =
            Error.Conflict(
                "Prescription.CannotDispenseCancelled",
                "Cannot dispense a cancelled prescription.");

        public static readonly Error CannotCancelDispensed =
            Error.Conflict(
                "Prescription.CannotCancelDispensed",
                "Cannot cancel a dispensed prescription.");

        public static readonly Error InvalidRefillCount =
            Error.Validation(
                "Prescription.InvalidRefillCount",
                "Refill count cannot be negative.");

        public static readonly Error PrescriptionExpired =
            Error.Conflict(
                "Prescription.Expired",
                "Prescription has expired and cannot be dispensed.");

        public static readonly Error DuplicatePrescription =
            Error.Conflict(
                "Prescription.Duplicate",
                "A prescription with the same medication already exists for this appointment.");
    }
}