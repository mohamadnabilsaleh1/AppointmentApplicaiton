using System.IO;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Dtos;
using AppointmentApplication.Application.Features.Users.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Users.GetAvatar
{
    public class GetAvatarQueryHandler : IRequestHandler<GetAvatarQuery, Result<FileUploadResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<GetAvatarQueryHandler> _logger;

        public GetAvatarQueryHandler(
            IAppDbContext context, 
            IFileStorageService fileStorageService,
            ILogger<GetAvatarQueryHandler> logger)
        {
            _context = context;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task<Result<FileUploadResponse>> Handle(GetAvatarQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(p => p.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User not found. ID: {UserId}", request.UserId);
                return ApplicationUserErrors.UserNotFound(request.UserId);
            }

            // Check if user has an avatar
            if (string.IsNullOrEmpty(user.Avatar))
            {
                _logger.LogWarning("User does not have an avatar. ID: {UserId}", request.UserId);
                return ApplicationUserErrors.AvatarNotFound(request.UserId);
            }

            // Extract file name from AvatarUrl
            var fileName = Path.GetFileName(user.Avatar);

            // Get the actual file bytes
            var filePath = $"users/{user.Id}/uploads/{fileName}";
            var fileBytes = await _fileStorageService.GetFileAsync(filePath);

            if (fileBytes == null || fileBytes.Length == 0)
            {
                _logger.LogWarning("Avatar file not found in storage. Path: {FilePath}", filePath);
                return ApplicationUserErrors.AvatarNotFound(request.UserId);
            }

            // Determine content type based on file extension
            var contentType = GetContentType(fileName);
            Console.WriteLine("filenmjsadfasdfadsf==================>");
            Console.WriteLine(contentType);
            // Return the file response
            return new FileUploadResponse
            {
                FileBytes = fileBytes,
                FileName = fileName,
                ContentType = contentType
            };
        }

        private static string GetContentType(string fileName)
        {

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }
}