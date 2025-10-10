using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Uploads.Dtos;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Patients.Uploads.Queries.GetUploadedFileByUserIdQuery
{
    public sealed record GetUploadedFilesByUserIdQuery(Guid UserId) : IRequest<Result<List<UploadDto>>>;
}