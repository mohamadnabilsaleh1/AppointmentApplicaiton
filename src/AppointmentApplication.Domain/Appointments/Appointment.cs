using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Billings;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities.Departments;
using AppointmentApplication.Domain.Patients;
using AppointmentApplication.Domain.Prescriptions;

namespace AppointmentApplication.Domain.Appointments;

public class Appointment:AuditableEntity
{
    private Appointment() { }

    public Guid PatientID { get; private set; }
    public Guid DoctorID { get; private set; }
    public Guid FacilityID { get; private set; }
    public Guid DepartmentID { get; private set; }
    public DateTime ScheduledDate { get; private set; }
    public TimeSpan ScheduledTime { get; private set; }
    public int DurationMinutes { get; private set; }
    public string Status { get; private set; }
    public DateTime BookingDate { get; private set; }
    public DateTime? CheckOutTime { get; private set; }
    public string Notes { get; private set; }
    public string CancelledReason { get; private set; }
    public Guid? RescheduledFrom { get; private set; }

    public Patient Patient { get; private set; }
    public Doctor Doctor { get; private set; }
    public HealthCareFacility Facility { get; private set; }
    public Department Department { get; private set; }

    private readonly List<Prescription> _prescriptions = new();
    public IReadOnlyCollection<Prescription> Prescriptions => _prescriptions.AsReadOnly();

    private readonly List<Billing> _billings = new();
    public IReadOnlyCollection<Billing> Billings => _billings.AsReadOnly();

    public static Appointment Create(Guid patientId, Guid doctorId, Guid facilityId, Guid departmentId,
        DateTime scheduledDate, TimeSpan scheduledTime, int durationMinutes, string notes)
    {
        return new Appointment
        {
            PatientID = patientId,
            DoctorID = doctorId,
            FacilityID = facilityId,
            DepartmentID = departmentId,
            ScheduledDate = scheduledDate,
            ScheduledTime = scheduledTime,
            DurationMinutes = durationMinutes,
            Status = "Pending",
            BookingDate = DateTime.UtcNow,
            Notes = notes,
        };
    }

    public void Confirm()
    {
        Status = "Confirmed";
    }

    public void Cancel(string reason)
    {
        Status = "Cancelled";
        CancelledReason = reason;
    }

    public void Complete()
    {
        Status = "Completed";
        CheckOutTime = DateTime.UtcNow;
    }

    public void MarkAsNoShow()
    {
        Status = "NoShow";
    }

    public void Reschedule(Guid rescheduledFromId, DateTime newDate, TimeSpan newTime)
    {
        Status = "Rescheduled";
        RescheduledFrom = rescheduledFromId;
        ScheduledDate = newDate;
        ScheduledTime = newTime;
    }
}
