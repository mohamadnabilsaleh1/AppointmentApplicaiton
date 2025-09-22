using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;

namespace AppointmentApplication.Domain.Doctors.Specializations;

public class Specialization:AuditableEntity
{
    private Specialization() { }

    public string Name { get; private set; }
    public string Description { get; private set; }

    private readonly List<Doctor> _doctors = new();
    public IReadOnlyCollection<Doctor> Doctors => _doctors.AsReadOnly();

    public static Specialization Create(string name, string description)
    {
        return new Specialization
        {
            Name = name,
            Description = description
        };
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
