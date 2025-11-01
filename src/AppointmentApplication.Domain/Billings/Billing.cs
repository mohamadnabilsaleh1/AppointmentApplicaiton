using System;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Billings.Enums;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.Patients;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Billings
{
    public class Billing : AuditableEntity
    {
        private Billing() { }

        public Guid AppointmentId { get; private set; }
        public Guid PatientId { get; private set; }
        public Guid DoctorId { get; private set; }
        public DateTime DateIssued { get; private set; }
        public decimal TotalAmount { get; private set; }
        public BillingStatus Status { get; private set; }
        public DateTime? PaymentDate { get; private set; }
        public decimal? PaidAmount { get; private set; }

        // Navigation Properties
        public virtual Appointment Appointment { get; private set; }
        public virtual Patient Patient { get; private set; }
        public virtual Doctor Doctor { get; private set; }

        // Factory Method
        public static Result<Billing> Create(
            Guid patientId,
            Guid appointmentId,
            Guid doctorId,
            decimal totalAmount)
        {
            // Domain validation
            if (totalAmount <= 0)
            {
                return BillingErrors.InvalidTotalAmount;
            }

            var billing = new Billing
            {
                Id = Guid.NewGuid(),
                PatientId = patientId,
                AppointmentId = appointmentId,
                DoctorId = doctorId,
                DateIssued = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = BillingStatus.Pending
            };

            return billing;
        }

        // Mark as Paid
        public Result<Updated> MarkAsPaid(decimal paidAmount)
        {
            if (Status == BillingStatus.Paid)
            {
                return BillingErrors.BillingAlreadyPaid;
            }

            if (paidAmount <= 0)
            {
                return BillingErrors.InvalidPaidAmount;
            }

            if (paidAmount != TotalAmount)
            {
                return BillingErrors.PartialPaymentNotAllowed;
            }

            Status = BillingStatus.Paid;
            PaymentDate = DateTime.UtcNow;
            PaidAmount = paidAmount;
            UpdatedAtUtc = DateTime.UtcNow;

            return Result.Updated;
        }

        // Cancel Billing
        public Result<Updated> Cancel()
        {
            if (Status == BillingStatus.Pending)
            {
                return BillingErrors.CannotCancelPaidBilling;
            }

            Status = BillingStatus.Cancelled;
            UpdatedAtUtc = DateTime.UtcNow;

            return Result.Updated;
        }

        // Check if billing can be completed
        public bool CanBeCompleted()
        {
            return Status == BillingStatus.Pending;
        }
    }
}