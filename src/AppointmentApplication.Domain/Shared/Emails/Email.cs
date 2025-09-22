using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;

namespace AppointmentApplication.Domain.Shared;

public class Email : AuditableEntity
{
    public string OwnerType { get; private set; }
    public Guid OwnerId { get; private set; }
    public string EmailAddress { get; private set; }
    public string Label { get; private set; } = string.Empty;
    public static Email Create(string ownerType, Guid ownerId, string emailAddress, string label )
    {
        return new Email
        {
            Id = Guid.NewGuid(),
            OwnerType = ownerType,
            OwnerId = ownerId,
            EmailAddress = emailAddress,
            Label = label,
        };
    }

}
