using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Application.Abstractions.Authentication;


public interface IJwtService
{
    Task<Result<string>> GetAccessTokenAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);
}