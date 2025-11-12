using AppointmentApplication.Application.Features.Emails.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Emails.Commands.SetPrimaryEmail
{
    public class SetPrimaryEmailCommandHandler(
        ILogger<SetPrimaryEmailCommandHandler> logger,
        IAppDbContext context)
        : IRequestHandler<SetPrimaryEmailCommand, Result<Updated>>
    {
        private readonly ILogger<SetPrimaryEmailCommandHandler> _logger = logger;
        private readonly IAppDbContext _context = context;

        public async Task<Result<Updated>> Handle(SetPrimaryEmailCommand request, CancellationToken cancellationToken)
        {
            // 🔹 تحميل المستخدم مع جميع إيميلاته
            var user = await _context.Users
                .Include(u => u.Emails)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                _logger.LogWarning("User not found. UserId: {UserId}", request.UserId);
                return ApplicationEmailErrors.UserNotFound(request.UserId);
            }

            // البحث عن الإيميل المطلوب
            var targetEmail = user.Emails.FirstOrDefault(e => e.Id == request.EmailId);
            if (targetEmail is null)
            {
                _logger.LogWarning("Email not found. EmailId: {EmailId}, UserId: {UserId}", 
                    request.EmailId, request.UserId);
                return ApplicationEmailErrors.EmailNotFound(request.EmailId);
            }

            // 🔹 إذا كان الإيميل بالفعل أساسي، لا حاجة للتحديث
            if (targetEmail.IsPrimary)
            {
                _logger.LogInformation("Email is already primary. EmailId: {EmailId}, UserId: {UserId}", 
                    request.EmailId, request.UserId);
                return Result.Updated;
            }

            // 🔹 إزالة الأساسية من جميع الإيميلات الأخرى أولاً
            var otherPrimaryEmails = user.Emails
                .Where(e => e.IsPrimary && e.Id != request.EmailId)
                .ToList();

            foreach (var email in otherPrimaryEmails)
            {
                var setPrimaryResult = email.SetPrimary(false);
                if (setPrimaryResult.IsError)
                {
                    _logger.LogWarning("Failed to remove primary status from email. EmailId: {EmailId}, Error: {Error}", 
                        email.Id, string.Join(", ", setPrimaryResult.Errors));
                    return setPrimaryResult.Errors;
                }
            }

            // 🔹 ثم جعل الإيميل المطلوب أساسي
            var setTargetPrimaryResult = targetEmail.SetPrimary(true);
            if (setTargetPrimaryResult.IsError)
            {
                _logger.LogWarning("Failed to set email as primary. EmailId: {EmailId}, Error: {Error}", 
                    request.EmailId, string.Join(", ", setTargetPrimaryResult.Errors));
                return setTargetPrimaryResult.Errors;
            }

            // 🔹 التحقق النهائي من عدم وجود أكثر من إيميل أساسي

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Email set as primary successfully. EmailId: {EmailId}, UserId: {UserId}", 
                request.EmailId, request.UserId);

            return Result.Updated;
        }
    }
}