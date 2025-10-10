using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Patients.Uploads.Commands.UpdateUploadFile
{
    public sealed record UpdateUploadFileCommand(Guid UserId, Guid UploadId, string Title, string Description) : IRequest<Result<Updated>>;
    
}