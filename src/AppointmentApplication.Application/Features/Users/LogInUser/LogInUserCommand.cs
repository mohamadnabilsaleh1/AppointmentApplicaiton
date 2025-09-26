using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Users.LogInUser;

public sealed record LogInUserCommand(
    string Email,
    string Password
) : IRequest<Result<AccessTokenResponse>>;