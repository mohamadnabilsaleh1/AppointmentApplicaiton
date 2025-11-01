using System;
using System.Collections.Generic;
using System.Linq;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Appointments.Enums;
using AppointmentApplication.Domain.Appointments.Errors;
using AppointmentApplication.Domain.Billings;
using AppointmentApplication.Domain.Billings.Enums;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Patients;
using AppointmentApplication.Domain.Prescriptions;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Appointments
{
    public class Appointment : AuditableEntity
    {
        public Guid PatientId { get; private set; }
        public Guid DoctorId { get; private set; }
        public Guid FacilityId { get; private set; }
        public Guid? BillingId { get; private set; }

        public DateOnly ScheduledDate { get; private set; }
        public TimeSpan ScheduledTime { get; private set; }
        public int DurationMinutes { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public DateTime BookingDate { get; private set; }
        public DateTime? CheckInTime { get; private set; }
        public DateTime? CheckOutTime { get; private set; }
        public string? Notes { get; private set; }
        public string CancellationReason { get; private set; }

        // Navigation Properties
        public virtual Patient Patient { get; private set; }
        public virtual Doctor Doctor { get; private set; }
        public virtual HealthCareFacility Facility { get; private set; }
        public virtual Billing Billing { get; private set; }

        private readonly List<Prescription> _prescriptions = new();
        public virtual IReadOnlyCollection<Prescription> Prescriptions => _prescriptions.AsReadOnly();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private Appointment() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        private Appointment(
            Guid id,
            Guid patientId,
            Guid doctorId,
            Guid facilityId,
            DateOnly scheduledDate,
            TimeSpan scheduledTime,
            int durationMinutes)
            : base(id)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            FacilityId = facilityId;
            ScheduledDate = scheduledDate;
            ScheduledTime = scheduledTime;
            DurationMinutes = durationMinutes;
            Status = AppointmentStatus.Pending;
            BookingDate = DateTime.UtcNow;

            CancellationReason = string.Empty;
        }

        // ✅ Factory Method
        public static Result<Appointment> Create(
            Guid patientId,
            Guid doctorId,
            Guid facilityId,
            DateOnly scheduledDate,
            TimeSpan scheduledTime,
            int durationMinutes)
        {
            // Domain validation
            if (patientId == Guid.Empty)
            {
                return AppointmentErrors.InvalidPatientId;
            }

            if (doctorId == Guid.Empty)
            {
                return AppointmentErrors.InvalidDoctorId;
            }

            if (facilityId == Guid.Empty)
            {
                return AppointmentErrors.InvalidFacilityId;
            }

            if (scheduledDate < DateOnly.FromDateTime(DateTime.Today))
            {
                return AppointmentErrors.InvalidScheduledDate;
            }

            if (scheduledDate > DateOnly.FromDateTime(DateTime.Today.AddYears(1)))
            {
                return AppointmentErrors.InvalidScheduledDate;
            }

            if (scheduledTime < TimeSpan.Zero || scheduledTime >= TimeSpan.FromHours(24))
            {
                return AppointmentErrors.InvalidScheduledTime;
            }

            // Business hours validation (8 AM to 8 PM)
            if (scheduledTime < TimeSpan.FromHours(8) || scheduledTime > TimeSpan.FromHours(20))
            {
                return AppointmentErrors.InvalidScheduledTime;
            }

            if (durationMinutes < 15 || durationMinutes > 480)
            {
                return AppointmentErrors.InvalidDuration;
            }

            var appointment = new Appointment(
                Guid.NewGuid(),
                patientId,
                doctorId,
                facilityId,
                scheduledDate,
                scheduledTime,
                durationMinutes
                );

            return appointment;
        }

        // ✅ Factory Method with Billing
        public static Result<Appointment> CreateWithBilling(
            Guid patientId,
            Guid doctorId,
            Guid facilityId,
            DateOnly scheduledDate,
            TimeSpan scheduledTime,
            int durationMinutes,
            decimal totalAmount)
        {
            var appointmentResult = Create(
                patientId, doctorId, facilityId, scheduledDate, scheduledTime, durationMinutes);

            if (appointmentResult.IsError)
            {
                return appointmentResult.Errors;
            }

            var appointment = appointmentResult.Value;

            var billingResult = Billing.Create(
                patientId, appointment.Id, doctorId, totalAmount);

            if (billingResult.IsError)
            {
                return billingResult.Errors;
            }

            var billing = billingResult.Value;
            appointment.AttachBilling(billing);

            return appointment;
        }

        // ✅ Confirm Appointment
        public Result<Updated> Confirm()
        {
            if (Status != AppointmentStatus.Pending)
            {
                return AppointmentErrors.InvalidStatusTransition;
            }


            Status = AppointmentStatus.Confirmed;
            CheckInTime = DateTime.UtcNow;
            UpdatedAtUtc = DateTime.UtcNow;

            return Result.Updated;
        }

        // ✅ Complete Appointment
        public Result<Updated> Complete()
        {
            if (Status != AppointmentStatus.Confirmed)
            {
                return AppointmentErrors.CannotCompleteWithoutConfirmation;
            }


            Status = AppointmentStatus.Completed;
            CheckOutTime = DateTime.UtcNow;
            UpdatedAtUtc = DateTime.UtcNow;
            Billing.MarkAsPaid(Billing.TotalAmount);
            return Result.Updated;
        }

        // ✅ Cancel Appointment
        public Result<Updated> Cancel(string reason)
        {
            if (Status == AppointmentStatus.Completed || Status == AppointmentStatus.Cancelled)
            {
                return AppointmentErrors.CannotCancelCompleted;
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                return AppointmentErrors.EmptyCancellationReason;
            }

            Status = AppointmentStatus.Cancelled;
            CancellationReason = reason.Trim();
            UpdatedAtUtc = DateTime.UtcNow;
            return Result.Updated;
        }

        // ✅ Mark as No Show
        public Result<Updated> MarkAsNoShow()
        {
            if (Status != AppointmentStatus.Confirmed && Status != AppointmentStatus.Pending)
            {
                return AppointmentErrors.InvalidStatusTransition;
            }

            Status = AppointmentStatus.NoShow;
            UpdatedAtUtc = DateTime.UtcNow;

            return Result.Updated;
        }

        // ✅ Reschedule Appointment
        public Result<Updated> Reschedule(DateOnly newDate, TimeSpan newTime)
        {
            if (Status == AppointmentStatus.Completed || Status == AppointmentStatus.Cancelled)
            {
                return AppointmentErrors.CannotRescheduleCompleted;
            }

            if (newDate < DateOnly.FromDateTime(DateTime.Today))
            {
                return AppointmentErrors.InvalidScheduledDate;
            }

            if (newTime < TimeSpan.FromHours(8) || newTime > TimeSpan.FromHours(20))
            {
                return AppointmentErrors.InvalidScheduledTime;
            }

            ScheduledDate = newDate;
            ScheduledTime = newTime;

            // Reset status to pending if it was confirmed
            if (Status == AppointmentStatus.Confirmed)
            {
                Status = AppointmentStatus.Pending;
                CheckInTime = null;
            }

            UpdatedAtUtc = DateTime.UtcNow;
            return Result.Updated;
        }

        // ✅ Update Notes
        public Result<Updated> UpdateNotes(string notes)
        {
            if (string.IsNullOrWhiteSpace(notes))
            {
                return AppointmentErrors.EmptyNotes;
            }

            if (notes.Length > 1000)
            {
                return AppointmentErrors.NotesTooLong;
            }

            Notes = notes.Trim();
            UpdatedAtUtc = DateTime.UtcNow;

            return Result.Updated;
        }

        // ✅ Add Prescription
        // public Result<Prescription> AddPrescription(
        //     Guid id,
        //     string medicationName,
        //     string dosage,
        //     string frequency,
        //     string duration,
        //     string instructions)
        // {
        //     var prescriptionResult = Prescription.Create(
        //         id, Id, medicationName, dosage, frequency, duration, instructions);

        //     if (prescriptionResult.IsError)
        //     {
        //         return prescriptionResult.Errors;
        //     }

        //     var prescription = prescriptionResult.Value;
        //     _prescriptions.Add(prescription);

        //     return prescription;
        // }

        // // ✅ Update Prescription
        // public Result<Updated> UpdatePrescription(
        //     Guid prescriptionId,
        //     string medicationName,
        //     string dosage,
        //     string frequency,
        //     string duration,
        //     string instructions)
        // {
        //     var prescription = _prescriptions.FirstOrDefault(p => p.Id == prescriptionId);
        //     if (prescription == null)
        //     {
        //         return PrescriptionErrors.PrescriptionNotFound;
        //     }

        //     var updateResult = prescription.Update(medicationName, dosage, frequency, duration, instructions);
        //     if (updateResult.IsError)
        //     {
        //         return updateResult.Errors;
        //     }

        //     return Result.Updated;
        // }

        // // ✅ Remove Prescription
        // public Result<Deleted> RemovePrescription(Guid prescriptionId)
        // {
        //     var prescription = _prescriptions.FirstOrDefault(p => p.Id == prescriptionId);
        //     if (prescription == null)
        //     {
        //         return PrescriptionErrors.PrescriptionNotFound;
        //     }

        //     _prescriptions.Remove(prescription);
        //     return Result.Deleted;
        // }

        // // ✅ Get Prescription by ID
        // public Result<Prescription> GetPrescriptionById(Guid prescriptionId)
        // {
        //     var prescription = _prescriptions.FirstOrDefault(p => p.Id == prescriptionId);
        //     if (prescription == null)
        //     {
        //         return PrescriptionErrors.PrescriptionNotFound;
        //     }

        //     return prescription;
        // }

        // ✅ Get All Prescriptions
        public Result<IReadOnlyCollection<Prescription>> GetAllPrescriptions()
        {
            return _prescriptions.AsReadOnly();
        }

        // ✅ Attach Billing (Internal)
        internal void AttachBilling(Billing billing)
        {
            Billing = billing;
            BillingId = billing.Id;
        }

        // ✅ Check if appointment is upcoming
        public bool IsUpcoming()
        {
            return ScheduledDate >= DateOnly.FromDateTime(DateTime.Today) &&
                   Status == AppointmentStatus.Confirmed;
        }

        // ✅ Check if appointment is past due
        public bool IsPastDue()
        {
            return ScheduledDate < DateOnly.FromDateTime(DateTime.Today) &&

                   Status == AppointmentStatus.Pending;
        }

        // ✅ Check if appointment can be modified
        public bool CanBeModified()
        {
            return Status == AppointmentStatus.Pending || Status == AppointmentStatus.Confirmed;
        }

        // ✅ Check if appointment is completed
        public bool IsCompleted()
        {
            return Status == AppointmentStatus.Completed;
        }

        // ✅ Check if appointment is cancelled
        public bool IsCancelled()
        {
            return Status == AppointmentStatus.Cancelled;
        }

        // ✅ Get appointment end time
        public TimeSpan GetEndTime()
        {
            return ScheduledTime.Add(TimeSpan.FromMinutes(DurationMinutes));
        }

        // ✅ Check if appointment time conflicts with another appointment
        public bool HasTimeConflict(Appointment other)
        {
            if (ScheduledDate != other.ScheduledDate)
                return false;

            var thisEndTime = GetEndTime();
            var otherEndTime = other.GetEndTime();

            return ScheduledTime < otherEndTime && other.ScheduledTime < thisEndTime;
        }
    }
}