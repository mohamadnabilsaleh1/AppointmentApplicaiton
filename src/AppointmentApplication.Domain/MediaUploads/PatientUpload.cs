using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.MediaUploads.Enums;
using AppointmentApplication.Domain.Patients;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.MediaUploads;

public class PatientUpload : AuditableEntity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private PatientUpload() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Guid PatientId { get; private set; }
    public string FileType { get; private set; }
    public string FileURL { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Visibility Visibility { get; private set; }

    public Patient? Patient { get; set; }

    private PatientUpload(Guid patientId, string fileType, string fileUrli,
        string title, string description, Visibility visibility = Visibility.Public)
    {
        PatientId = patientId;
        FileType = fileType;
        FileURL = fileUrli;
        Title = title;
        Description = description;
        Visibility = visibility;
    }

    public static Result<PatientUpload> Create(Guid patientId, string fileType, string fileUrli,
        string title, string description, Visibility visibility = Visibility.Public)
    {
        return new PatientUpload(patientId, fileType, fileUrli, title, description, visibility);
    }

    public Result<Updated> Update(string title, string description)
    {
        Title = title;
        Description = description;
        return Result.Updated;
    }

    public Result<Updated> ChangeVisibilityToPublic()
    {
        Visibility = Visibility.Public;
        return Result.Updated;
    }
    public Result<Updated> ChangeVisibilityToPrivate()
    {
        Visibility = Visibility.Private;
        return Result.Updated;
    }
}
