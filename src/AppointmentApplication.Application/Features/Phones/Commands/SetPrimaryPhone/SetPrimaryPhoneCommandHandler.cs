// AppointmentApplication.Application/Features/Phones/Commands/SetPrimaryPhone/SetPrimaryPhoneCommandHandler.cs
using AppointmentApplication.Application.Features.Phones.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Phones.Commands.SetPrimaryPhone
{
    public class SetPrimaryPhoneCommandHandler(
        ILogger<SetPrimaryPhoneCommandHandler> logger,
        IAppDbContext context)
        : IRequestHandler<SetPrimaryPhoneCommand, Result<Updated>>
    {
        private readonly ILogger<SetPrimaryPhoneCommandHandler> _logger = logger;
        private readonly IAppDbContext _context = context;

        public async Task<Result<Updated>> Handle(SetPrimaryPhoneCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.Phones)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                _logger.LogWarning("User not found. UserId: {UserId}", request.UserId);
                return ApplicationPhoneErrors.UserNotFound(request.UserId);
            }

            try
            {
                user.SetPrimaryPhone(request.PhoneId);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Primary phone set successfully. Phone: {Phone}, UserId: {UserId}", 
                    request.PhoneId, request.UserId);

                return Result.Updated;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(
                    "Failed to set primary phone. Phone: {Phone}, UserId: {UserId}, Error: {Error}", 
                    request.PhoneId, request.UserId, ex.Message);
                return ApplicationPhoneErrors.PhoneNumberNotFound(request.PhoneId);
            }
        }
    }
}