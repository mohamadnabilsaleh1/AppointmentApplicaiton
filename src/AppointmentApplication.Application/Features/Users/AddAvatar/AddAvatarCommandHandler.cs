using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Errors;
using AppointmentApplication.Application.Features.Users.Errors;

using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Users.AddAvatar
{
    public class AddAvatarCommandHandler : IRequestHandler<AddAvatarCommand, Result<Updated>>
    {
        private readonly ILogger<AddAvatarCommandHandler> _logger;
        private readonly IAppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public AddAvatarCommandHandler(
            ILogger<AddAvatarCommandHandler> logger,
            IAppDbContext context,
            IFileStorageService fileStorageService)
        {
            _logger = logger;
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<Updated>> Handle(AddAvatarCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users
                .FirstOrDefault(p => p.Id == request.UserId);

            if (user is null)
            {
                _logger.LogWarning("User not found. ID: {UserId}", request.UserId);
                return ApplicationUserErrors.UserNotFound(request.UserId);
            }

            // Save the file and get the file name/path
            var fileName = await _fileStorageService.SaveFileAsync(
                request.File,
                $"users/{user.Id}/uploads");
            // Create the file URL (you might want to configure this base URL)
            var fileUrl = $"/api/users/{user.Id}/uploads/{fileName}";

            var uploadResult = user.SetAvatar(fileUrl);

            if (uploadResult.IsError)
            {
                _logger.LogWarning("Failed to Add Avatar: {Error}", uploadResult.Errors);
                return uploadResult.Errors;
            }

            // Add to context and save changes
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Avatar uploaded successfully for  {UserId}",
                user.Id);

            return Result.Updated;
        }
    }
}