using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities;

namespace AppointmentApplication.Domain.DoctorFacilities;

public class DoctorFacility : AuditableEntity
{
    public Guid DoctorId { get; private set; }
    public Guid FacilityId { get; private set; }
    public bool IsActive { get; private set; }

    public Doctor Doctor { get; private set; }
    public HealthCareFacility Facility { get; private set; }
    private DoctorFacility() { }
    public static DoctorFacility Create(Guid doctorId, Guid facilityId)
    {
        return new DoctorFacility
        {
            DoctorId = doctorId,
            FacilityId = facilityId,
            IsActive = true,
        };
    }
}
