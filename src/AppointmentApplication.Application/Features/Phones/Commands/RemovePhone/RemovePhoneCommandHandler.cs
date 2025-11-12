// AppointmentApplication.Application/Features/Phones/Commands/RemovePhone/RemovePhoneCommandHandler.cs
using AppointmentApplication.Application.Features.Phones.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Phones.Commands.RemovePhone
{
    public class RemovePhoneCommandHandler(
        ILogger<RemovePhoneCommandHandler> logger,
        IAppDbContext context)
        : IRequestHandler<RemovePhoneCommand, Result<Deleted>>
    {
        private readonly ILogger<RemovePhoneCommandHandler> _logger = logger;
        private readonly IAppDbContext _context = context;

        public async Task<Result<Deleted>> Handle(RemovePhoneCommand request, CancellationToken cancellationToken)
        {
            // Option A: Load user with phones and remove from collection
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
                _logger.LogWarning("Phone not found. PhoneId: {PhoneId}, UserId: {UserId}", 
                    request.PhoneId, request.UserId);
                return ApplicationPhoneErrors.PhoneNotFound(request.PhoneId);
            }

            // Remove the phone from user's collection
            user.RemovePhone(request.PhoneId);
            
            // Also remove from context to ensure it's tracked for deletion
            _context.Phones.Remove(phone);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Phone removed successfully. PhoneId: {PhoneId}, UserId: {UserId}", 
                request.PhoneId, request.UserId);

            return Result.Deleted;
        }
    }
}