using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Emails.Dtos;

using AppointmentApplication.Domain.Emails;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Emails.Commands.AddEmail
{
    public record AddEmailCommand(Guid UserId, string EmailAddress, string Label, bool IsPrimary = false) : IRequest<Result<EmailDto>>;
}