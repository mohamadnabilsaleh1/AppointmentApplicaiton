using System;
using System.Collections.Generic;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.MedicalRecordAttachments;
using AppointmentApplication.Domain.Patients;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.MedicalRecords
{
    public class MedicalRecord : AuditableEntity
    {
        private MedicalRecord() { }

        public Guid PatientId { get; private set; }
        public Guid FacilityId { get; private set; }
        public Guid DoctorId { get; private set; }
        public Guid AppointmentId { get; private set; }
        public DateTime RecordDate { get; private set; }
        public string Diagnosis { get; private set; }
        public string TreatmentNotes { get; private set; }
        public string FollowUpInstructions { get; private set; }

        // Navigation Properties
        public virtual Patient? Patient { get; private set; }
        public virtual HealthCareFacility? Facility { get; private set; }
        public virtual Doctor? Doctor { get; private set; }
        public virtual Appointment? Appointment { get; private set; }

        private readonly List<MedicalRecordAttachment> _attachments = new();
        public virtual IReadOnlyCollection<MedicalRecordAttachment> Attachments => _attachments.AsReadOnly();

        // Factory Method
        public static Result<MedicalRecord> Create(
            Guid patientId,
            Guid facilityId,
            Guid doctorId,
            Guid appointmentId,
            string diagnosis,
            string treatmentNotes,
            string followUpInstructions = "")
        {
            // Domain validation
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                return MedicalRecordErrors.EmptyDiagnosis;
            }

            if (string.IsNullOrWhiteSpace(treatmentNotes))
            {
                return MedicalRecordErrors.EmptyTreatmentNotes;
            }

            if (diagnosis.Length > 500)
            {
                return MedicalRecordErrors.DiagnosisTooLong;
            }

            if (treatmentNotes.Length > 2000)
            {
                return MedicalRecordErrors.TreatmentNotesTooLong;
            }

            if (followUpInstructions.Length > 1000)
            {
                return MedicalRecordErrors.FollowUpInstructionsTooLong;
            }

            var medicalRecord = new MedicalRecord
            {
                Id = Guid.NewGuid(),
                PatientId = patientId,
                FacilityId = facilityId,
                DoctorId = doctorId,
                AppointmentId = appointmentId,
                RecordDate = DateTime.UtcNow,
                Diagnosis = diagnosis.Trim(),
                TreatmentNotes = treatmentNotes.Trim(),
                FollowUpInstructions = followUpInstructions?.Trim() ?? ""
            };

            return medicalRecord;
        }

        // Add Attachment
        // public Result<MedicalRecordAttachment> AddAttachment(
        //     Guid uploadedById,
        //     string fileType,
        //     string fileUrl,
        //     string title,
        //     string description,
        //     string visibility = "Private")
        // {
        //     var attachmentResult = MedicalRecordAttachment.Create(
        //         Id, uploadedById, fileType, fileUrl, title, description, visibility);

        //     if (attachmentResult.IsError)
        //     {
        //         return attachmentResult.Errors;
        //     }

        //     _attachments.Add(attachmentResult.Value);
        //     return attachmentResult.Value;
        // }

        // Update Medical Record
        public Result<Updated> Update(
            string diagnosis,
            string treatmentNotes,
            string followUpInstructions)
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                return MedicalRecordErrors.EmptyDiagnosis;
            }

            if (string.IsNullOrWhiteSpace(treatmentNotes))
            {
                return MedicalRecordErrors.EmptyTreatmentNotes;
            }

            Diagnosis = diagnosis.Trim();
            TreatmentNotes = treatmentNotes.Trim();
            FollowUpInstructions = followUpInstructions?.Trim() ?? "";
            UpdatedAtUtc = DateTime.UtcNow;

            return Result.Updated;
        }
    }
}