using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Patients;
using System.Collections.Generic;

namespace AppointmentApplication.Domain.Users;

public sealed class User : AuditableEntity
{
    private User(Guid id, string firstName, string lastName, string email)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

#pragma warning disable CS8618
    private User() { } // For EF Core
#pragma warning restore CS8618

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string IdentityId { get; private set; } = string.Empty;

    // 👇 One-to-Many: User -> HealthCareFacilities
    private readonly List<HealthCareFacility> _healthCareFacilities = new();
    public IReadOnlyCollection<HealthCareFacility> HealthCareFacilities => _healthCareFacilities.AsReadOnly();

    // 👇 One-to-Many: User -> Patients
    private readonly List<Patient> _patients = new();
    public IReadOnlyCollection<Patient> Patients => _patients.AsReadOnly();

    // 👇 One-to-Many: User -> Doctors
    private readonly List<Doctor> _doctors = new();
    public IReadOnlyCollection<Doctor> Doctors => _doctors.AsReadOnly();

    // 👇 Many-to-Many: User -> Roles
    private readonly List<Role> _roles = new();
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    public static User Create(Guid id, string firstName, string lastName, string email,Role role)
    {
        var user = new User(id, firstName, lastName, email);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        user._roles.Add(role);

        return user;
    }

    public void SetIdentityId(string identityId)
    {
        IdentityId = identityId;
    }

    // Optional helper methods to add related entities
    public void AddHealthCareFacility(HealthCareFacility facility)
    {
        if (!_healthCareFacilities.Contains(facility))
        {
            _healthCareFacilities.Add(facility);
        }
    }

    public void AddPatient(Patient patient)
    {
        if (!_patients.Contains(patient))
        {
            _patients.Add(patient);
        }
    }

    public void AddDoctor(Doctor doctor)
    {
        if (!_doctors.Contains(doctor))
        {
            _doctors.Add(doctor);
        }
    }
}
