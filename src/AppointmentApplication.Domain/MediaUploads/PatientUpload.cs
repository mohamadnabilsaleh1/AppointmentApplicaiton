using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Patients;

namespace AppointmentApplication.Domain.MediaUploads;

public class PatientUpload : AuditableEntity
{
    private PatientUpload() { }

    public Guid PatientId { get; private set; }
    public string FileType { get; private set; }
    public string FileURL { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Visibility { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime UploadedAt { get; private set; }

    public Patient Patient { get; private set; }

    public static PatientUpload Create(Guid patientId, string fileType, string fileUrli,
        string title, string description, string visibility = "Public")
    {
        return new PatientUpload
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
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
