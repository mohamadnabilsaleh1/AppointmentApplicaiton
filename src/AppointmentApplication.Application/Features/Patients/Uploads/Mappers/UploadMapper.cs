using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Uploads.Dtos;
using AppointmentApplication.Domain.MediaUploads;

namespace AppointmentApplication.Application.Features.Patients.Uploads.Mappers
{
    public static class UploadMapper
    {
        public static UploadDto ToDto(this PatientUpload entity)
        {
            return new UploadDto(
                entity.Id,
                entity.PatientId,
                entity.FileType,
                entity.FileURL,
                entity.Title,
                entity.Description);
        }

        public static List<UploadDto> ToDtos(this IEnumerable<PatientUpload> entities)
        {
            return entities.Select(e => e.ToDto()).ToList();
        }
    }
}

