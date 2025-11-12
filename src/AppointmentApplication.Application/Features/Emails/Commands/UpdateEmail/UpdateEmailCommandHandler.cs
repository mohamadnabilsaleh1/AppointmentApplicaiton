using AppointmentApplication.Application.Features.Emails.Dtos;
using AppointmentApplication.Application.Features.Emails.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Emails.Commands.UpdateEmail
{
    public class UpdateEmailCommandHandler(
        ILogger<UpdateEmailCommandHandler> logger,
        IAppDbContext context)
        : IRequestHandler<UpdateEmailCommand, Result<Updated>>
    {
        private readonly ILogger<UpdateEmailCommandHandler> _logger = logger;
        private readonly IAppDbContext _context = context;

        public async Task<Result<Updated>> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
        {
            // 🔹 تحميل المستخدم مع جميع إيميلاته للتعامل مع الأساسية
            var user = await _context.Users
                .Include(u => u.Emails)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                _logger.LogWarning("User not found. UserId: {UserId}", request.UserId);
                return ApplicationEmailErrors.UserNotFound(request.UserId);
            }

            var email = user.Emails.FirstOrDefault(e => e.Id == request.EmailId);
            if (email is null)
            {
                _logger.LogWarning(
                    "Email not found. EmailId: {EmailId}, UserId: {UserId}", 
                    request.EmailId, request.UserId);
                return ApplicationEmailErrors.EmailNotFound(request.EmailId);
            }

            // 🔹 إذا كنا نجعل هذا الإيميل أساسي، نزيل الأساسية من الآخرين أولاً
            if (request.IsPrimary.HasValue && request.IsPrimary.Value && !email.IsPrimary)
            {
                var otherPrimaryEmails = user.Emails
                    .Where(e => e.IsPrimary && e.Id != request.EmailId)
                    .ToList();

                foreach (var otherEmail in otherPrimaryEmails)
                {
                    var setPrimaryResult = otherEmail.SetPrimary(false);
                    if (setPrimaryResult.IsError)
                    {
                        _logger.LogWarning(
                            "Failed to remove primary status from email. EmailId: {EmailId}, Error: {Error}",
                            otherEmail.Id, string.Join(", ", setPrimaryResult.Errors));
                        return setPrimaryResult.Errors;
                    }
                }
            }

            // 🔹 تحديث عنوان الإيميل إذا تم تقديمه وصحيح
            if (!string.IsNullOrWhiteSpace(request.EmailAddress) && request.EmailAddress != email.EmailAddress)
            {
                // التحقق من عدم وجود تكرار
                var isDuplicate = user.Emails.Any(e =>
                    e.EmailAddress == request.EmailAddress && e.Id != request.EmailId);
                
                if (isDuplicate)
                {
                    _logger.LogWarning(
                        "Email address already exists. Email: {Email}, UserId: {UserId}",
                        request.EmailAddress, request.UserId);
                    return ApplicationEmailErrors.EmailAlreadyExists(request.EmailAddress);
                }

                var updateResult = email.UpdateEmailAddress(request.EmailAddress, "system");
                if (updateResult.IsError)
                {
                    _logger.LogWarning(
                        "Failed to update email address. EmailId: {EmailId}, Error: {Error}",
                        request.EmailId, string.Join(", ", updateResult.Errors));
                    return updateResult.Errors;
                }
            }

            // 🔹 تحديث التصنيف إذا تم تقديمه
            if (!string.IsNullOrWhiteSpace(request.Label) && request.Label != email.Label)
            {
                var updateResult = email.UpdateLabel(request.Label, "system");
                if (updateResult.IsError)
                {
                    _logger.LogWarning(
                        "Failed to update email label. EmailId: {EmailId}, Error: {Error}",
                        request.EmailId, string.Join(", ", updateResult.Errors));
                    return updateResult.Errors;
                }
            }

            // 🔹 تحديث حالة الأساسية
            if (request.IsPrimary.HasValue && request.IsPrimary.Value != email.IsPrimary)
            {
                var setPrimaryResult = email.SetPrimary(request.IsPrimary.Value);
                if (setPrimaryResult.IsError)
                {
                    _logger.LogWarning(
                        "Failed to set email as primary. EmailId: {EmailId}, Error: {Error}",
                        request.EmailId, string.Join(", ", setPrimaryResult.Errors));
                    return setPrimaryResult.Errors;
                }
            }

            // 🔹 التحقق من عدم وجود أكثر من إيميل أساسي

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Email updated successfully. EmailId: {EmailId}, UserId: {UserId}",
                request.EmailId, request.UserId);

            return Result.Updated;
        }
    }
}