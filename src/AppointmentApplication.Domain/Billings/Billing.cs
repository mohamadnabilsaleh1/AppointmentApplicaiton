using System;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Billings.BillingPayments;
using AppointmentApplication.Domain.Billings.Enums;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.Patients;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Billings;

public class Billing : AuditableEntity
{
    private Billing() { }

    public Guid AppointmentID { get; private set; }
    public Guid? BillingPaymentID { get; private set; } // Optional until paid
    public Guid PatientID { get; private set; }
    public Guid DoctorID { get; private set; }

    public DateTime DateIssued { get; private set; }
    public decimal TotalAmount { get; private set; }
    public BillingStatus Status { get; private set; }
    public string Notes { get; private set; }

    // 🔗 Navigation Properties
    public Appointment Appointment { get; private set; }
    public BillingPayment? BillingPayment { get; private set; }
    public Patient Patient { get; private set; }
    public Doctor Doctor { get; private set; }

    // 🏗 Factory Method
    public static Result<Billing> Create(Guid patientId, Guid appointmentId, Guid doctorId,
        decimal totalAmount, string notes)
    {
        return new Billing
        {
            Id = Guid.NewGuid(),
            PatientID = patientId,
            AppointmentID = appointmentId,
            DoctorID = doctorId,
            DateIssued = DateTime.UtcNow,
            TotalAmount = totalAmount,
            Status = BillingStatus.Pending,
            Notes = notes,
        };
    }

    // ⚙️ Behavior Methods
    // public void UpdateStatus(App status)
    // {
    //     Status = status;
    // }

    public void AttachPayment(BillingPayment payment)
    {
        BillingPayment = payment;
        BillingPaymentID = payment.Id;
    }
}
