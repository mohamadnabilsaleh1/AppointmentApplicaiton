using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Users;

namespace AppointmentApplication.Application.Abstractions.Authentication;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(
        User user,
        string password,
        CancellationToken cancellationToken = default);
}