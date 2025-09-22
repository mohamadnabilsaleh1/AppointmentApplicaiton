using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Patients;

namespace AppointmentApplication.Domain.Reviews;

public class Review : AuditableEntity
{
    private Review() { }

    public Guid PatientID { get; private set; }
    public Guid? FacilityID { get; private set; }
    public Guid? DoctorID { get; private set; }
    public int Rating { get; private set; }
    public string Comment { get; private set; }

    public Patient Patient { get; private set; }
    public HealthCareFacility Facility { get; private set; }
    public Doctor Doctor { get; private set; }

    public static Review Create(Guid patientId, int rating, string comment,
        Guid? facilityId = null, Guid? doctorId = null)
    {
        return new Review
        {
            Id = Guid.NewGuid(),
            PatientID = patientId,
            FacilityID = facilityId,
            DoctorID = doctorId,
            Rating = rating,
            Comment = comment,
        };
    }

    public void Update(int rating, string comment)
    {
        Rating = rating;
        Comment = comment;
    }
}
