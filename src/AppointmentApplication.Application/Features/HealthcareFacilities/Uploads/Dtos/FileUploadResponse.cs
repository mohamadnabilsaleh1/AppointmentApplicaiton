using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Dtos
{
    public class FileUploadResponse
    {
        public UploadDto Upload { get; set; } = null!;
        public byte[] FileBytes { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
    }
}