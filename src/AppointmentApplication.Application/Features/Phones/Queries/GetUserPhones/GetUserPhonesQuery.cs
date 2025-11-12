using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Phones.Dtos;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Phones.Queries.GetUserPhones
{
    public sealed record GetUserPhonesQuery(Guid UserId) : IRequest<Result<List<PhoneDto>>>;
}