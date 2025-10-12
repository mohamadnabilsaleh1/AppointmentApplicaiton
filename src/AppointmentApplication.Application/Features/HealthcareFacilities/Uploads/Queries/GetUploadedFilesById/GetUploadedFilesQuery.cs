using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Dtos;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Commands.ChangeFileToPrivate
{
    public sealed record GetUploadedFilesQuery(Guid HealthCareFacilityId) : IRequest<Result<List<UploadDto>>>;
}