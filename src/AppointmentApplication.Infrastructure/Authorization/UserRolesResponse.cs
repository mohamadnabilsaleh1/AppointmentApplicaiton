using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Users;

namespace AppointmentApplication.Infrastructure.Authorization;

internal sealed class UserRolesResponse
{
    public Guid UserId { get; init; }

    public List<Role> Roles { get; init; } = [];
}