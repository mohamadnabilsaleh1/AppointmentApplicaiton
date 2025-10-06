using System;
using System.Collections.Generic;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.DoctorDepartments;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.HealthcareFacilities.Departments;

public sealed class Department : AuditableEntity
{
    public Guid FacilityId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public HealthCareFacility? HealthcareFacility { get;  set; }

    private readonly List<DoctorDepartment> _doctorDepartments = new();
    public IReadOnlyCollection<DoctorDepartment> DoctorDepartments => _doctorDepartments.AsReadOnly();

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

    // ✅ Doctor management
    public Result<Success> AddDoctorDepartment(DoctorDepartment doctorDepartment)
    {
        if (doctorDepartment is null)
        {
            return DepartmentErrors.DoctorDepartmentRequired;
        }

        _doctorDepartments.Add(doctorDepartment);
        return Result.Success;
    }

    public Result<Success> RemoveDoctorDepartment(DoctorDepartment doctorDepartment)
    {
        if (doctorDepartment is null)
        {
            return DepartmentErrors.DoctorDepartmentRequired;
        }

        _doctorDepartments.Remove(doctorDepartment);
        return Result.Success;
    }

}
