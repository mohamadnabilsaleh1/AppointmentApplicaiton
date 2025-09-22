using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Doctors;

namespace AppointmentApplication.Domain.Prescriptions;

public class Prescription:AuditableEntity
{
    private Prescription() { }

    public Guid AppointmentID { get; private set; }
    public Guid IssuedByDoctorID { get; private set; }
    public DateTime DateIssued { get; private set; }
    public string MedicationList { get; private set; }
    public string DosageInstructions { get; private set; }
    public string Status { get; private set; }

    public Appointment Appointment { get; private set; }
    public Doctor Doctor { get; private set; }

    public static Prescription Create(Guid appointmentId, Guid doctorId, string medicationList,
        string dosageInstructions, string status = "Active")
    {
        return new Prescription
        {
            AppointmentID = appointmentId,
            IssuedByDoctorID = doctorId,
            DateIssued = DateTime.UtcNow,
            MedicationList = medicationList,
            DosageInstructions = dosageInstructions,
            Status = status,
        };
    }

    public void Update(string medicationList, string dosageInstructions, string status)
    {
        MedicationList = medicationList;
        DosageInstructions = dosageInstructions;
        Status = status;
    }
}
