using System;
using System.Collections.Generic;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.HealthcareFacilities.Departments;

public sealed class Department : AuditableEntity
{
    public Guid FacilityId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public HealthCareFacility? HealthcareFacility { get; set; }

    private readonly List<Doctor> _doctors = new();
    public IReadOnlyCollection<Doctor> Doctors => _doctors.AsReadOnly();

#pragma warning disable CS8618
    private Department() { }
#pragma warning restore CS8618

    private Department(Guid healthcareFacilityId, string name, string description)
    {
        Id = Guid.NewGuid();
        FacilityId = healthcareFacilityId;
        Name = name;
        Description = description;
    }

    // ✅ Create
    public static Result<Department> Create(Guid healthcareFacilityId, string name, string description)
    {
        if (healthcareFacilityId == Guid.Empty)
        {
            return DepartmentErrors.FacilityIdRequired;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return DepartmentErrors.NameRequired;
        }

        if (!string.IsNullOrWhiteSpace(description) && description.Length > 1000)
        {
            return DepartmentErrors.DescriptionTooLong;
        }

        return new Department(healthcareFacilityId, name.Trim(), description?.Trim() ?? string.Empty);
    }

    // ✅ Update
    public Result<Updated> Update(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return DepartmentErrors.NameRequired;
        }

        if (!string.IsNullOrWhiteSpace(description) && description.Length > 1000)
        {
            return DepartmentErrors.DescriptionTooLong;
        }

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;

        return Result.Updated;
    }
    public Result<Success> AddDoctor(Doctor doctor)
    {
        if (_doctors.Any(d => d.Id == doctor.Id))
        {
            return DoctorErrors.DoctorAlreadyExists;
        }

        _doctors.Add(doctor);
        return Result.Success;
    }

    public Result<Success> RemoveDoctor(Guid doctorId)
    {
        var doctor = _doctors.FirstOrDefault(d => d.Id == doctorId);
        if (doctor is null)
        {
            return DoctorErrors.DoctorNotFound;
        }
        _doctors.Remove(doctor);
        return Result.Success;
    }
    public Result<Doctor> GetDoctor(Guid doctorId)
    {
        var doctor = _doctors.FirstOrDefault(d => d.Id == doctorId);
        if (doctor is null)
        {
            return DoctorErrors.DoctorNotFound;
        }
        return doctor;
    }
}
