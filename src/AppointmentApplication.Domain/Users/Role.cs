namespace AppointmentApplication.Domain.Users;

public sealed class Role
{
    public static readonly Role Patient = new(1, "Patient");
    public static readonly Role Admin = new(2, "Admin");
    public static readonly Role HealthCareFacility = new(3, "HealthCareFacility");
    public static readonly Role Doctor = new(4, "Doctor");

    public Role(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }

    public string Name { get; init; }

    public ICollection<User> Users { get; init; } = new List<User>();

    public ICollection<Permission> Permissions { get; init; } = new List<Permission>();
}
