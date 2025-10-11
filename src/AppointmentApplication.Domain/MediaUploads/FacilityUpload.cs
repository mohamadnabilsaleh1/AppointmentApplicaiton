using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.MediaUploads.Enums;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.MediaUploads;

public class FacilityUpload : AuditableEntity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private FacilityUpload() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public Guid FacilityId { get; private set; }
    public string FileType { get; private set; }
    public string FileURL { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Visibility Visibility { get; private set; }

    public HealthCareFacility Facility { get; private set; }

    public static Result<FacilityUpload> Create(Guid facilityId, string fileType, string fileUrli,
        string title, string description, Visibility visibility = Visibility.Public)
    {
        return new FacilityUpload
        {
            FacilityId = facilityId,
            FileType = fileType,
            FileURL = fileUrli,
            Title = title,
            Description = description,
            Visibility = visibility,
        };
    }

    public Result<Updated> Update(string title, string description)
    {
        Title = title;
        Description = description;
        return Result.Updated;
    }
    public Result<Updated> ChangeUploadVisibilityToPrivate()
    {
        Visibility = Visibility.Public;
        return Result.Updated;
    }
    public Result<Updated> ChangeUploadVisibilityToPublic()
    {
        Visibility = Visibility.Private;
        return Result.Updated;
    }

}
