using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Application.Features.Patients.Uploads.Dtos
{
    public sealed record UploadDto(Guid Id,Guid PatientId, string FileType, string FileURL, string Title, string Description);
}

