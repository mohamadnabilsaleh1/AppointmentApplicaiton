using AppointmentApplication.Application.Features.Phones.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Phones.Commands.UpdatePhone
{
    public class UpdatePhoneCommandHandler(
        ILogger<UpdatePhoneCommandHandler> logger,
        IAppDbContext context)
        : IRequestHandler<UpdatePhoneCommand, Result<Updated>>
    {
        private readonly ILogger<UpdatePhoneCommandHandler> _logger = logger;
        private readonly IAppDbContext _context = context;

        public async Task<Result<Updated>> Handle(UpdatePhoneCommand request, CancellationToken cancellationToken)
        {
            // 🔹 استخدام إستراتيجية التنفيذ لإعادة المحاولة



            // 🔹 إعادة تحميل البيانات داخل ال transaction
            var user = await _context.Users
                .Include(u => u.Phones)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                _logger.LogWarning("User not found. UserId: {UserId}", request.UserId);
                return ApplicationPhoneErrors.UserNotFound(request.UserId);
            }

            var phone = user.Phones.FirstOrDefault(p => p.Id == request.PhoneId);
            if (phone is null)
            {
                _logger.LogWarning(
                    "Phone not found. PhoneId: {PhoneId}, UserId: {UserId}",
                    request.PhoneId, request.UserId);
                return ApplicationPhoneErrors.PhoneNotFound(request.PhoneId);
            }

            // 🔹 إذا كنا نجعل هذا الهاتف أساسي، نتعامل مع الأساسية أولاً بشكل منفصل
            if (request.IsPrimary.HasValue && request.IsPrimary.Value && !phone.IsPrimary)
            {
                // 1. إزالة الأساسية من جميع الهواتف الأخرى أولاً
                var otherPrimaryPhones = user.Phones
                    .Where(p => p.IsPrimary && p.Id != request.PhoneId)
                    .ToList();

                foreach (var otherPhone in otherPrimaryPhones)
                {
                    otherPhone.SetPrimary(false);
                }

                // 2. حفظ التغييرات أولاً (إزالة الأساسية من الآخرين)
                await _context.SaveChangesAsync(cancellationToken);

                // 3. ثم جعل الهاتف المطلوب أساسي
                phone.SetPrimary(true);

                // 4. حفظ التغييرات مرة أخرى
                await _context.SaveChangesAsync(cancellationToken);
            }

            // تحديث رقم الهاتف إذا تم تقديمه وصحيح
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber) && request.PhoneNumber != phone.PhoneNumber)
            {
                // التحقق من عدم وجود تكرار
                var isDuplicate = user.Phones.Any(p =>
                    p.PhoneNumber == request.PhoneNumber && p.Id != request.PhoneId);

                if (isDuplicate)
                {
                    _logger.LogWarning(
                        "Phone number already exists. Phone: {Phone}, UserId: {UserId}",
                        request.PhoneNumber, request.UserId);
                    return ApplicationPhoneErrors.PhoneAlreadyExists(request.PhoneNumber);
                }

                phone.UpdatePhoneNumber(request.PhoneNumber, "system");
            }

            // تحديث التصنيف إذا تم تقديمه
            if (!string.IsNullOrWhiteSpace(request.Label) && request.Label != phone.Label)
            {
                phone.UpdateLabel(request.Label, "system");
            }

            // 🔹 إذا لم يكن التحديث متعلقاً بالأساسية، نحفظ مرة واحدة
            if (!(request.IsPrimary.HasValue && request.IsPrimary.Value && !phone.IsPrimary))
            {
                await _context.SaveChangesAsync(cancellationToken);
            }

            // 🔹 التحقق النهائي من عدم وجود أكثر من هاتف أساسي
            var primaryPhoneCount = user.Phones.Count(p => p.IsPrimary);
            if (primaryPhoneCount > 1)
            {
                _logger.LogError("Multiple primary phones detected after update. UserId: {UserId}, Count: {Count}",
                    request.UserId, primaryPhoneCount);
                return ApplicationPhoneErrors.MultiplePrimaryPhones(request.UserId);
            }


            _logger.LogInformation(
                "Phone updated successfully. PhoneId: {PhoneId}, UserId: {UserId}",
                request.PhoneId, request.UserId);

            return Result.Updated;
        }
    }
}
