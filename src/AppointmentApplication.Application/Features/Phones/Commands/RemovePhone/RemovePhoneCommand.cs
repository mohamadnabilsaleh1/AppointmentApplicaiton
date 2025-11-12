using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Phones.Commands.RemovePhone
{
    public record RemovePhoneCommand(
        Guid UserId,
        Guid PhoneId
    ) : IRequest<Result<Deleted>>;
}