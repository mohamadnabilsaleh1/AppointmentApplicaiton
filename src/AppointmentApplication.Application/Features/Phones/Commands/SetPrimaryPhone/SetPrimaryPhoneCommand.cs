using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Phones.Commands.SetPrimaryPhone
{
    public record SetPrimaryPhoneCommand(
        Guid UserId,
        Guid PhoneId
    ) : IRequest<Result<Updated>>;
}