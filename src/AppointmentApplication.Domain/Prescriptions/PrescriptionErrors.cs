using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Prescriptions
{
    public static class PrescriptionErrors
    {
        public static readonly Error PrescriptionNotFound =
            Error.NotFound("Prescription.NotFound", "Prescription not found.");

        public static readonly Error EmptyMedicationList =
            Error.Validation("Prescription.EmptyMedicationList", "Medication list cannot be empty.");

        public static readonly Error EmptyDosageInstructions =
            Error.Validation("Prescription.EmptyDosageInstructions", "Dosage instructions cannot be empty.");

        public static readonly Error MedicationListTooLong =
            Error.Validation("Prescription.MedicationListTooLong", "Medication list cannot exceed 1000 characters.");

        public static readonly Error DosageInstructionsTooLong =
            Error.Validation("Prescription.DosageInstructionsTooLong", "Dosage instructions cannot exceed 1000 characters.");
    }
}