using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.HealthcareFacilities;

namespace AppointmentApplication.Domain.MediaUploads;

public class FacilityUpload:AuditableEntity
{
    private FacilityUpload() { }

    public Guid FacilityId { get; private set; }
    public string FileType { get; private set; }
    public string FileURL { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Visibility { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime UploadedAt { get; private set; }

    public HealthCareFacility Facility { get; private set; }

    public static FacilityUpload Create(Guid facilityId, string fileType, string fileUrli,
        string title, string description, string visibility = "Public")
    {
        return new FacilityUpload
        {
            FacilityId = facilityId,
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
