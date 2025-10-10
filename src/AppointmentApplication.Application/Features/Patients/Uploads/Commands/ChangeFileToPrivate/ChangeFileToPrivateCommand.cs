using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Patients.Uploads.Commands.ChangeFileToPrivate
{
    public sealed record ChangeFileToPrivateCommand(Guid UserId, Guid UploadId) : IRequest<Result<Updated>>;
}