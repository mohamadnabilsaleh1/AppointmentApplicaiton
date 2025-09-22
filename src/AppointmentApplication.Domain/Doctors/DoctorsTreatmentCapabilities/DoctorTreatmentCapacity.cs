using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;

namespace AppointmentApplication.Domain.Doctors.DoctorsTreatmentCapabilities;

public class DoctorTreatmentCapacity : AuditableEntity
{
    private DoctorTreatmentCapacity() { }

    public Guid DoctorId { get; private set; }
    public int MaxPatientsPerDay { get; private set; }
    public int SessionDurationMinutes { get; private set; }
    public bool IsActive { get; private set; }

    public Doctor Doctor { get; private set; }

    public static DoctorTreatmentCapacity Create(Guid doctorId, int maxPatientsPerDay = 10,
        int sessionDurationMinutes = 30)
    {
        return new DoctorTreatmentCapacity
        {
            DoctorId = doctorId,
            MaxPatientsPerDay = maxPatientsPerDay,
            SessionDurationMinutes = sessionDurationMinutes,
            IsActive = true,
        };
    }
}
