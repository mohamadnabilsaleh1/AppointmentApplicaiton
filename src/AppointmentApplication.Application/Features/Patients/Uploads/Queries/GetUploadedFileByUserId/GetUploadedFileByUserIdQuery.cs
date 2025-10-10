using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Uploads.Dtos;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Patients.Uploads.Queries.GetUploadedFileByUserId
{
    public sealed record GetUploadedFileByUserIdQuery(Guid UserId, Guid UploadedId) : IRequest<Result<UploadDto>>;
}