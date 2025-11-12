using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Phones.Dtos;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Phones.Commands.AddPhone
{
  public record AddPhoneCommand(Guid UserId, string PhoneNumber, string Label, bool IsPrimary = false) 
        : IRequest<Result<PhoneDto>>;
}