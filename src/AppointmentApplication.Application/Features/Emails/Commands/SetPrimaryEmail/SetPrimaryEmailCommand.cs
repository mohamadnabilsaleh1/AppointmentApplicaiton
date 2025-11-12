using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Emails.Commands.SetPrimaryEmail
{
    public record SetPrimaryEmailCommand(
        Guid UserId,
        Guid EmailId
    ) : IRequest<Result<Updated>>;
}