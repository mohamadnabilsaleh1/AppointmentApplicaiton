using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Dtos;

using AppointmentApplication.Domain.MediaUploads;

namespace AppointmentApplication.Application.HealthcareFacilities.Patients.Uploads.Mappers
{
    public static class UploadMapper
    {
        public static UploadDto ToDto(this FacilityUpload entity)
        {
            var uploadFolder = Path.Combine("file:///home/kali/Desktop/AppointmentApplication/Uploads", "healthCareFacility", entity.FacilityId.ToString(), "uploads");
            var uploadedFile = entity.FileURL.Split("/");
            var fileId = uploadedFile[uploadedFile.Length - 1];
            var filePath = Path.Combine(uploadFolder, $"{fileId}");
            //api/files/healthCareFacility/08ab65af-cdb4-4f2f-910f-cd45a1ba5eaf/uploads/bb6538a4-4bf1-4d94-b612-36a9c8738afa.png
            //

            return new UploadDto(
                entity.Id,
                entity.FacilityId,
                entity.FileType,
                entity.FileURL,
                entity.Title,
                entity.Description,
                filePath);
        }

        public static List<UploadDto> ToDtos(this IEnumerable<FacilityUpload> entities)
        {
            return entities.Select(e => e.ToDto()).ToList();
        }
    }
}

/*

                // var result = await _sender.Send(new GetUploadedFileByIdQuery(facilityId, id), cancellationToken);
                // Console.WriteLine(result.Value);

                //file:///home/kali/Desktop/AppointmentApplication/Uploads/healthCareFacility/08ab65af-cdb4-4f2f-910f-cd45a1ba5eaf/uploads/83fb9bb5-62d1-45c2-9448-6b6e093a88e8.png
                // var file = result.Value;
                // var filePath = $"/home/kali/Desktop/AppointmentApplication/Uploads/healthCareFacility/{facilityId}/uploads/{file.Id}.{GetContentType(file.FileType)}";
                var uploadFolder = Path.Combine("Uploads", "healthCareFacility", facilityId.ToString(), "uploads");
                // var filePath = Path.Combine(uploadFolder, $"{file.Id}{Path.GetExtension(file.FileURL)}");
                var filePath = Path.Combine(uploadFolder, $"{"83fb9bb5-62d1-45c2-9448-6b6e093a88e8"}.png");

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath, cancellationToken);
                var contentType = GetContentType("png");

                return File(fileBytes, contentType);
*/
