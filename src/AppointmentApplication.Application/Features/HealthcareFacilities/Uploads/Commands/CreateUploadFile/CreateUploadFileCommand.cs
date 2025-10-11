using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.MediaUploads;

using AppointmentApplication.Domain.MediaUploads.Enums;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.AspNetCore.Http;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Commands.ChangeFileToPrivate
{
    public sealed record CreateUploadFileCommand(Guid UserId, IFormFile File,
        string Title, string Description, Visibility Visibility = Visibility.Public) : IRequest<Result<FacilityUpload>>;
}