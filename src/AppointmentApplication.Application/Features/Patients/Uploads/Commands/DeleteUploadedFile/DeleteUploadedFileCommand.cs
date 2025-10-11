using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Patients.Uploads.Commands.DeleteUploadedFile
{
    public sealed record DeleteUploadedFileCommand(Guid UserId, Guid FileId) : IRequest<Result<Deleted>>;
}