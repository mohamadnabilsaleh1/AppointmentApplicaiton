using System;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Prescriptions
{
    public class Prescription : AuditableEntity
    {
        private Prescription() { }

        public Guid AppointmentId { get; private set; }
        public Guid DoctorId { get; private set; }
        public DateTime DateIssued { get; private set; }
        public string MedicationList { get; private set; }
        public string DosageInstructions { get; private set; }

        // Navigation Properties
        public virtual Appointment Appointment { get; private set; }
        public virtual Doctor Doctor { get; private set; }

        // Factory Method
        public static Result<Prescription> Create(
            Guid appointmentId,
            Guid doctorId,
            string medicationList,
            string dosageInstructions)
        {
            // Domain validation
            if (string.IsNullOrWhiteSpace(medicationList))
            {
                return PrescriptionErrors.EmptyMedicationList;
            }

            if (string.IsNullOrWhiteSpace(dosageInstructions))
            {
                return PrescriptionErrors.EmptyDosageInstructions;
            }

            if (medicationList.Length > 1000)
            {
                return PrescriptionErrors.MedicationListTooLong;
            }

            if (dosageInstructions.Length > 1000)
            {
                return PrescriptionErrors.DosageInstructionsTooLong;
            }

            var prescription = new Prescription
            {
                Id = Guid.NewGuid(),
                AppointmentId = appointmentId,
                DoctorId = doctorId,
                DateIssued = DateTime.UtcNow,
                MedicationList = medicationList.Trim(),
                DosageInstructions = dosageInstructions.Trim()
            };

            return prescription;
        }

        // Update Method
        public Result<Updated> Update(string medicationList, string dosageInstructions)
        {
            if (string.IsNullOrWhiteSpace(medicationList))
            {
                return PrescriptionErrors.EmptyMedicationList;
            }

            if (string.IsNullOrWhiteSpace(dosageInstructions))
            {
                return PrescriptionErrors.EmptyDosageInstructions;
            }

            if (medicationList.Length > 1000)
            {
                return PrescriptionErrors.MedicationListTooLong;
            }

            if (dosageInstructions.Length > 1000)
            {
                return PrescriptionErrors.DosageInstructionsTooLong;
            }

            MedicationList = medicationList.Trim();
            DosageInstructions = dosageInstructions.Trim();
            UpdatedAtUtc = DateTime.UtcNow;

            return Result.Updated;
        }
    }
}