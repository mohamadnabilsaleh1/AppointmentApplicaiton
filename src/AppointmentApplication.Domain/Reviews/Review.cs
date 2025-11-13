using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Patients;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Reviews;

public class Review : AuditableEntity
{
    private Review() { }

    public Guid PatientID { get; private set; }
    public Guid FacilityID { get; private set; }
    public Guid DoctorID { get; private set; }
    public Guid AppointmentId { get; private set; }
    public int Rating { get; private set; }
    public string Comment { get; private set; }

    public Patient? Patient { get; private set; }
    public HealthCareFacility? Facility { get; private set; }
    public Doctor? Doctor { get; private set; }
    public Appointment? Appointment { get; private set; }

    private Review(
        Guid id,
        Guid patientId,
        Guid facilityId,
        Guid doctorId,
        Guid appointmentId,
        int rating,
        string comment)
        : base(id)
    {
        PatientID = patientId;
        FacilityID = facilityId;
        DoctorID = doctorId;
        AppointmentId = appointmentId;
        Rating = rating;
        Comment = comment;
    }

    public static Result<Review> Create(
        Guid patientId,
        Guid facilityId,
        Guid doctorId,
        Guid appointmentId,
        int rating,
        string? comment = null)
    {
        if (patientId == Guid.Empty)
        {
            return ReviewErrors.PatientIdRequired;
        }

        if (facilityId == Guid.Empty)
        {
            return ReviewErrors.FacilityIdRequired;
        }

        if (doctorId == Guid.Empty)
        {
            return ReviewErrors.DoctorIdRequired;
        }

        if (appointmentId == Guid.Empty)
        {
            return ReviewErrors.AppointmentIdRequired;
        }

        if (rating < 1 || rating > 5)
        {
            return ReviewErrors.InvalidRating;
        }

        if (!string.IsNullOrWhiteSpace(comment) && comment.Length > 1000)
        {
            return ReviewErrors.CommentTooLong;
        }

        return new Review(
            Guid.NewGuid(),
            patientId,
            facilityId,
            doctorId,
            appointmentId,
            rating,
            comment?.Trim() ?? string.Empty);
    }

    // Update method
    public Result<Updated> Update(int rating, string? comment = null)
    {
        if (rating < 1 || rating > 5)
        {
            return ReviewErrors.InvalidRating;
        }

        if (!string.IsNullOrWhiteSpace(comment) && comment.Length > 1000)
        {
            return ReviewErrors.CommentTooLong;
        }

        Rating = rating;
        Comment = comment?.Trim() ?? string.Empty;

        return Result.Updated;
    }
}