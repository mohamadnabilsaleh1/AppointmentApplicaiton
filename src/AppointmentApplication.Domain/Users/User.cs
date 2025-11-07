using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Patients;
using AppointmentApplication.Domain.Emails;
using AppointmentApplication.Domain.Phones;
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
    private readonly List<Email> _emails = new();
    private readonly List<Phone> _phones = new();
    public IReadOnlyCollection<Email> Emails => _emails.AsReadOnly();
    public IReadOnlyCollection<Phone> Phones => _phones.AsReadOnly();

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

    public Result<Emails.Email> AddEmail(string emailAddress, string label, bool isPrimary = false, string createdBy = "system")
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            throw new ArgumentException("Email address cannot be empty", nameof(emailAddress));
        }

        if (string.IsNullOrWhiteSpace(label))
        {
            throw new ArgumentException("Label cannot be empty", nameof(label));
        }

        var email = Domain.Emails.Email.Create(Id, emailAddress, label, isPrimary);

        if (isPrimary)
        {
            // Remove primary status from other emails
            _emails.ForEach(e => e.SetPrimary(false));
        }

        if (email.IsError)
        {
            return email.Errors;
        }

        _emails.Add(email.Value);
        return email.Value;
    }

    public void RemoveEmail(string emailAddress)
    {
        var email = _emails.FirstOrDefault(e => e.EmailAddress == emailAddress);
        if (email != null)
        {
            _emails.Remove(email);
        }
    }

    public void SetPrimaryEmail(string emailAddress)
    {
        var email = _emails.FirstOrDefault(e => e.EmailAddress == emailAddress);
        if (email == null)
        {
            throw new InvalidOperationException("Email not found");
        }

        // Remove primary status from all emails
        _emails.ForEach(e => e.SetPrimary(false));

        // Set the specified email as primary
        email.SetPrimary(true);
    }

    // Phone methods
    public Result<Phone> AddPhone(string phoneNumber, string label, bool isPrimary = false, string createdBy = "system")
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));
        }

        if (string.IsNullOrWhiteSpace(label))
        {
            throw new ArgumentException("Label cannot be empty", nameof(label));
        }

        var phone = Phone.Create(Id, phoneNumber, label, isPrimary, createdBy);

        if (isPrimary)
        {
            // Remove primary status from other phones
            _phones.ForEach(p => p.SetPrimary(false));
        }

        if (phone.IsError)
        {
            return phone.Errors;
        }

        _phones.Add(phone.Value);
        return phone.Value;
    }

    public void RemovePhone(string phoneNumber)
    {
        var phone = _phones.FirstOrDefault(p => p.PhoneNumber == phoneNumber);
        if (phone != null)
        {
            _phones.Remove(phone);
        }
    }

    public void SetPrimaryPhone(string phoneNumber)
    {
        var phone = _phones.FirstOrDefault(p => p.PhoneNumber == phoneNumber);
        if (phone == null)
        {
            throw new InvalidOperationException("Phone not found");
        }

        // Remove primary status from all phones
        _phones.ForEach(p => p.SetPrimary(false));

        phone.SetPrimary(true);
    }

    // Helper methods
    public Email? GetPrimaryEmail() => _emails.FirstOrDefault(e => e.IsPrimary);
    public Phone? GetPrimaryPhone() => _phones.FirstOrDefault(p => p.IsPrimary);

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