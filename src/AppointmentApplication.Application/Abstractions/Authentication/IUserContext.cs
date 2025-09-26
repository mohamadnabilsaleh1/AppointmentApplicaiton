using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Application.Abstractions.Authentication;

public interface IUserContext
{
    Guid UserId { get; }

    string IdentityId { get; }
}