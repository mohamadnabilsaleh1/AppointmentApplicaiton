using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Doctors.DoctorsTreatmentCapabilities;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Doctors.DoctorsTreatmentCapabilities;

public class DoctorTreatmentCapacity : AuditableEntity
{
    public Guid DoctorId { get; private set; }
    public int MaxPatientsPerDay { get; private set; }
    public int SessionDurationMinutes { get; private set; }
    public bool IsActive { get; private set; }

    public Doctor Doctor { get; private set; } = null!;

    private DoctorTreatmentCapacity(Guid id, Guid doctorId, int maxPatientsPerDay,
        int sessionDurationMinutes, bool isActive)
        : base(id)
    {
        DoctorId = doctorId;
        MaxPatientsPerDay = maxPatientsPerDay;
        SessionDurationMinutes = sessionDurationMinutes;
        IsActive = isActive;
    }

    public static Result<DoctorTreatmentCapacity> Create(
        Guid doctorId,
        int maxPatientsPerDay,
        int sessionDurationMinutes)
    {
        if (doctorId == Guid.Empty)
        {
            return DoctorTreatmentCapacityErrors.DoctorIdRequired;
        }

        if (maxPatientsPerDay <= 0)
        {
            return DoctorTreatmentCapacityErrors.InvalidMaxPatients;
        }

        if (sessionDurationMinutes <= 0 || sessionDurationMinutes > 1440)
        {
            return DoctorTreatmentCapacityErrors.InvalidSessionDuration;
        }

        return new DoctorTreatmentCapacity(
            Guid.NewGuid(),
            doctorId,
            maxPatientsPerDay,
            sessionDurationMinutes,
            true);
    }

    public Result<Updated> Update(int maxPatientsPerDay, int sessionDurationMinutes)
    {
        if (maxPatientsPerDay <= 0)
        {
            return DoctorTreatmentCapacityErrors.InvalidMaxPatients;
        }

        if (sessionDurationMinutes <= 0 || sessionDurationMinutes > 1440)
        {
            return DoctorTreatmentCapacityErrors.InvalidSessionDuration;
        }

        MaxPatientsPerDay = maxPatientsPerDay;
        SessionDurationMinutes = sessionDurationMinutes;

        return Result.Updated;
    }

    public Result<Updated> Deactivate()
    {
        IsActive = false;
        return Result.Updated;
    }

    public Result<Updated> Activate()
    {
        IsActive = true;
        return Result.Updated;
    }

    public bool CanAcceptPatient(int currentPatientCount)
    {
        return IsActive && currentPatientCount < MaxPatientsPerDay;
    }

    public int CalculateTotalWorkingMinutes()
    {
        return MaxPatientsPerDay * SessionDurationMinutes;
    }

    public double CalculateTotalWorkingHours()
    {
        return CalculateTotalWorkingMinutes() / 60.0;
    }
}


