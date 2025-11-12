using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Emails.Commands.UpdateEmail
{
   public record UpdateEmailCommand(
        Guid UserId,
        Guid EmailId,
        string? EmailAddress = null,
        string? Label = null,
        bool? IsPrimary = null
    ) : IRequest<Result<Updated>>;
}