using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Patients;
using AppointmentApplication.Domain.Shared.Results;

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

    private readonly List<HealthCareFacility> _healthCareFacilities = new();
    public IReadOnlyCollection<HealthCareFacility> HealthCareFacilities => _healthCareFacilities.AsReadOnly();

    private readonly List<Patient> _patients = new();
    public IReadOnlyCollection<Patient> Patients => _patients.AsReadOnly();

    private readonly List<Doctor> _doctors = new();
    public IReadOnlyCollection<Doctor> Doctors => _doctors.AsReadOnly();

    private readonly List<Role> _roles = new();
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    public static Result<User> Create(Guid id, string firstName, string lastName, string email, Role role)
    {
        // 🔹 Validation
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return UserErrors.FirstNameRequired;
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return UserErrors.LastNameRequired;
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return UserErrors.EmailRequired;
        }

        if (!IsValidEmail(email))
        {
            return UserErrors.InvalidEmail;
        }

        if (role is null)
        {
            return UserErrors.RoleRequired;
        }

        // 🔹 Create new user
        var user = new User(id, firstName.Trim(), lastName.Trim(), email.Trim().ToLowerInvariant());

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        user._roles.Add(role);

        return user;
    }

    public void SetIdentityId(string identityId)
    {
        IdentityId = identityId;
    }

    // 🔹 Email validation (basic regex)
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
