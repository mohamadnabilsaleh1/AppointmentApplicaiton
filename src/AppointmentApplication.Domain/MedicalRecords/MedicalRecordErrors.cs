using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.MedicalRecords
{
    public static class MedicalRecordErrors
    {
        public static readonly Error MedicalRecordNotFound =
            Error.NotFound("MedicalRecord.NotFound", "Medical record not found.");

        public static readonly Error EmptyDiagnosis =
            Error.Validation("MedicalRecord.EmptyDiagnosis", "Diagnosis cannot be empty.");

        public static readonly Error EmptyTreatmentNotes =
            Error.Validation("MedicalRecord.EmptyTreatmentNotes", "Treatment notes cannot be empty.");

        public static readonly Error DiagnosisTooLong =
            Error.Validation("MedicalRecord.DiagnosisTooLong", "Diagnosis cannot exceed 500 characters.");

        public static readonly Error TreatmentNotesTooLong =
            Error.Validation("MedicalRecord.TreatmentNotesTooLong", "Treatment notes cannot exceed 2000 characters.");

        public static readonly Error FollowUpInstructionsTooLong =
            Error.Validation("MedicalRecord.FollowUpInstructionsTooLong", "Follow-up instructions cannot exceed 1000 characters.");
    }
}