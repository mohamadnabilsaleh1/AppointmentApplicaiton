using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.DoctorDepartments;
using AppointmentApplication.Domain.DoctorFacilities;
using AppointmentApplication.Domain.Doctors.ScheduleExceptions;
using AppointmentApplication.Domain.Doctors.Schedules;
using AppointmentApplication.Domain.Doctors.Specializations;
using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.Domain.Doctors;

public class Doctor : AuditableEntity
{
    public string UserId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Gender Gender { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public string LicenseNumber { get; private set; }
    public bool IsActive { get; private set; } = true;
    public Specialization Specialization { get; private set; }
    private readonly List<DoctorFacility> _facilities = new();
    public IReadOnlyCollection<DoctorFacility> Facilities => _facilities.AsReadOnly();

    private readonly List<DoctorDepartment> _departments = new();
    public IReadOnlyCollection<DoctorDepartment> Departments => _departments.AsReadOnly();

    private readonly List<ScheduleDoctor> _schedules = new();
    public IReadOnlyCollection<ScheduleDoctor> Schedules => _schedules.AsReadOnly();
    public Guid SpecializationID { get; private set; }
    private readonly List<ScheduleExceptionDoctor> _scheduleExceptions = new();
    public IReadOnlyCollection<ScheduleExceptionDoctor> ScheduleExceptions => _scheduleExceptions.AsReadOnly();

    // private readonly List<Appointment> _appointments = new();
    // public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();
    private Doctor() { }
    public static Doctor Create(string userId, string firstName, string lastName, Gender gender,
    DateOnly dateOfBirth, Guid specializationId, string licenseNumber)
    {
        return new Doctor
        {
            UserId = userId,
            FirstName = firstName,
            LastName = lastName,
            Gender = gender,
            DateOfBirth = dateOfBirth,
            SpecializationID = specializationId,
            LicenseNumber = licenseNumber,
            IsActive = true,
        };
    }
}
