using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.MedicalRecords;

namespace AppointmentApplication.Domain.MedicalRecordAttachments;

public class MedicalRecordAttachment:AuditableEntity
{
    private MedicalRecordAttachment() { }

    public Guid MedicalRecordID { get; private set; }
    public Guid UploadedByID { get; private set; }
    public string FileType { get; private set; }
    public string FileURL { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Visibility { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime UploadedAt { get; private set; }

    public MedicalRecord MedicalRecord { get; private set; }

    public static MedicalRecordAttachment Create(Guid medicalRecordId, Guid uploadedById,
        string fileType, string fileUrli, string title, string description,
        string visibility = "Private")
    {
        return new MedicalRecordAttachment
        {
            MedicalRecordID = medicalRecordId,
            UploadedByID = uploadedById,
            FileType = fileType,
            FileURL = fileUrli,
            Title = title,
            Description = description,
            Visibility = visibility,
            IsActive = true,
            UploadedAt = DateTime.UtcNow,
        };
    }

    public void Update(string title, string description, string visibility)
    {
        Title = title;
        Description = description;
        Visibility = visibility;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
