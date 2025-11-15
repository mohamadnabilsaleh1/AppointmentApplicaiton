using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.AspNetCore.Http;

namespace AppointmentApplication.Application.Features.Users.AddAvatar
{
    public sealed record AddAvatarCommand(Guid UserId, IFormFile File) : IRequest<Result<Updated>>;

}