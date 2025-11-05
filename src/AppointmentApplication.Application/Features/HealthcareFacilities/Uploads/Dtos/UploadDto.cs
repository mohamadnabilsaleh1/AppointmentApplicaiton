using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Dtos
{
    public sealed record UploadDto(Guid Id, Guid FacilityId, string FileType, string FileURL, string Title, string Description, string LocalPath = "");
}

