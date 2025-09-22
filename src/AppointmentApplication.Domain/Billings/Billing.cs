using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Billings.BillingPayments;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Patients;

namespace AppointmentApplication.Domain.Billings;

public class Billing:AuditableEntity
{
 private Billing() { }
    
    public Guid PatientID { get; private set; }
    public Guid FacilityID { get; private set; }
    public Guid AppointmentID { get; private set; }
    public Guid DoctorID { get; private set; }
    public DateTime DateIssued { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Status { get; private set; }
    public string Notes { get; private set; }
    
    public Patient Patient { get; private set; }
    public HealthCareFacility Facility { get; private set; }
    public Appointment Appointment { get; private set; }
    public Doctor Doctor { get; private set; }
    
    private readonly List<BillingPayment> _payments = new();
    public IReadOnlyCollection<BillingPayment> Payments => _payments.AsReadOnly();

    public static Billing Create(Guid patientId, Guid facilityId, Guid appointmentId, Guid doctorId, 
        decimal totalAmount, string notes)
    {
        return new Billing
        {
            PatientID = patientId,
            FacilityID = facilityId,
            AppointmentID = appointmentId,
            DoctorID = doctorId,
            DateIssued = DateTime.UtcNow,
            TotalAmount = totalAmount,
            Status = "Pending",
            Notes = notes,
        };
    }
    
    public void UpdateStatus(string status)
    {
        Status = status;
    }
    
    public void AddPayment(decimal amount) => TotalAmount += amount;
}
