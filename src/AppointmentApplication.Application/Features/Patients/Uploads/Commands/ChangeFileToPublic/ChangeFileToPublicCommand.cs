using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Patients.Uploads.Commands.ChangeFileToPublic
{
    public sealed record ChangeFileToPublicCommand(Guid UserId, Guid UploadId):IRequest<Result<Updated>>;

}