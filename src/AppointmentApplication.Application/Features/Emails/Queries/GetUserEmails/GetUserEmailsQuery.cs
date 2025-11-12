using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Emails.Dtos;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Emails.Queries.GetUserEmails
{
    public sealed record GetUserEmailsQuery(Guid UserId) : IRequest<Result<List<EmailDto>>>;
}