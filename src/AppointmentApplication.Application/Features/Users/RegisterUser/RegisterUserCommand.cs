using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Users.RegisterUser
{
    public sealed record RegisterUserCommand(string Email, string FirstName, string LastName, string Password) :IRequest<Result<Guid>>;
}