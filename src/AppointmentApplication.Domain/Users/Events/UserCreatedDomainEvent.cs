
using AppointmentApplication.Domain.Abstractions;

namespace AppointmentApplication.Domain.Users;

public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;
