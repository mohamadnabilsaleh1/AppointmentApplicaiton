using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities.Departments;

namespace AppointmentApplication.Domain.DoctorDepartments;

public class DoctorDepartment : AuditableEntity
{
    private DoctorDepartment() { }

    public Guid DoctorId { get; private set; }
    public Guid DepartmentId { get; private set; }
    public Guid FacilityId { get; private set; }
    public bool IsActive { get; private set; }

    public Doctor Doctor { get; private set; }
    public Department Department { get; private set; }
    public HealthCareFacility Facility { get; private set; }
    public static DoctorDepartment Create(Guid doctorId, Guid departmentId, Guid facilityId)
    {
        return new DoctorDepartment
        {
            DoctorId = doctorId,
            DepartmentId = departmentId,
            FacilityId = facilityId,
            IsActive = true,
        };
    }

}
