using AppointmentApplication.Domain.Abstractions;

namespace AppointmentApplication.Domain.Abstractions;

public abstract class AuditableEntity : Entity
{
    protected AuditableEntity()
    { }

    protected AuditableEntity(Guid id)
        : base(id)
    {
    }

    public DateTime CreatedAtUtc { get; set; } = DateTime.Now;

    public string? CreatedBy { get; set; }

    public DateTime UpdatedAtdUtc { get; set; }

    public string? LastModifiedBy { get; set; }
}