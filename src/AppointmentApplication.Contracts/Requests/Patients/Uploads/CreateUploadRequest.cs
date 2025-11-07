using AppointmentApplication.Domain.MediaUploads.Enums;

using Microsoft.AspNetCore.Http;

namespace AppointmentApplication.Contracts.Requests.Patients.Uploads
{
    public class CreateUploadRequest
    {
        public Guid UserId { get; set; }
        public required IFormFile File { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Visibility Visibility { get; set; } = Visibility.Public;
    }
}