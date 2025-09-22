using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;

namespace AppointmentApplication.Domain.Shared.Phone;

public class Phone : AuditableEntity
{
    public string OwnerType { get; private set; }
    public Guid OwnerId { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Label { get; private set; } = string.Empty;
    public static Phone Create(string ownerType, Guid ownerId, string phoneNumber, string label)
    {
        return new Phone
        {
            OwnerType = ownerType,
            OwnerId = ownerId,
            PhoneNumber = phoneNumber,
            Label = label,
        };
    }
}
