// AppointmentApplication.Application/Features/Emails/Commands/RemoveEmail/RemoveEmailCommandHandler.cs
using AppointmentApplication.Application.Features.Emails.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Emails.Commands.RemoveEmail
{
    public class RemoveEmailCommandHandler(
        ILogger<RemoveEmailCommandHandler> logger,
        IAppDbContext context)
        : IRequestHandler<RemoveEmailCommand, Result<Deleted>>
    {
        private readonly ILogger<RemoveEmailCommandHandler> _logger = logger;
        private readonly IAppDbContext _context = context;

        public async Task<Result<Deleted>> Handle(RemoveEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.Emails)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                _logger.LogWarning("User not found. UserId: {UserId}", request.UserId);
                return ApplicationEmailErrors.UserNotFound(request.UserId);
            }

            user.RemoveEmail(request.EmailId);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Email removed successfully. Email: {Email}, UserId: {UserId}", 
                request.EmailId, request.UserId);

            return Result.Deleted;
        }
    }
}