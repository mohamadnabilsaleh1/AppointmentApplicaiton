using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.MedicalRecordAttachments;
using AppointmentApplication.Domain.Patients;

namespace AppointmentApplication.Domain.MedicalRecords;

public class MedicalRecord:AuditableEntity
{
    private MedicalRecord() { }

    public Guid PatientID { get; private set; }
    public Guid FacilityID { get; private set; }
    public Guid DoctorID { get; private set; }
    public Guid? AppointmentID { get; private set; }
    public string RecordType { get; private set; }
    public DateTime DateCreated { get; private set; }
    public string Status { get; private set; }
    public string Details { get; private set; }
    public string Notes { get; private set; } = string.Empty;

    public Patient Patient { get; private set; }
    public HealthCareFacility Facility { get; private set; }
    public Doctor Doctor { get; private set; }
    public Appointment Appointment { get; private set; }

    private readonly List<MedicalRecordAttachment> _attachments = new();
    public IReadOnlyCollection<MedicalRecordAttachment> Attachments => _attachments.AsReadOnly();

    public static MedicalRecord Create(Guid patientId, Guid facilityId, Guid doctorId,
        string recordType, string details, Guid appointmentId, string notes)
    {
        return new MedicalRecord
        {
            PatientID = patientId,
            FacilityID = facilityId,
            DoctorID = doctorId,
            AppointmentID = appointmentId,
            RecordType = recordType,
            DateCreated = DateTime.UtcNow,
            Status = "Active",
            Details = details,
            Notes = notes,
        };
    }

    public void Update(string details, string notes)
    {
        Details = details;
        Notes = notes;
    }

    public void Archive() => Status = "Archived";
    public void Delete() => Status = "Deleted";
    public void Activate() => Status = "Active";
}
