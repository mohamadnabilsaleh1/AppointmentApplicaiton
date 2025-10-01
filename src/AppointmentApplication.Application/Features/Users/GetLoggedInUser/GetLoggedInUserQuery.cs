using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Users.Dtos;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Users.GetLoggedInUser
{
    // public sealed record GetHealthCareFacilityByIdQuery(Guid Id,string? Fields)
    // : IRequest<Result<ExpandoObject>>;
    public sealed record GetLoggedInUserQuery() : IRequest<Result<UserDto>>;

}