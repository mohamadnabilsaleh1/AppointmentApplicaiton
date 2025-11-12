using AppointmentApplication.Application.Features.Emails.Dtos;
using AppointmentApplication.Application.Features.Emails.Errors;
using AppointmentApplication.Application.Features.Emails.Mapper;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Emails.Commands.AddEmail
{
    public class AddEmailCommandHandler(
        ILogger<AddEmailCommandHandler> logger,
        IAppDbContext context)
        : IRequestHandler<AddEmailCommand, Result<EmailDto>>
    {
        private readonly ILogger<AddEmailCommandHandler> _logger = logger;
        private readonly IAppDbContext _context = context;

        public async Task<Result<EmailDto>> Handle(AddEmailCommand request, CancellationToken cancellationToken)
        {
                // Get user with emails included
                var user = await _context.Users
                    .Include(u => u.Emails)
                    .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

                if (user is null)
                {
                    _logger.LogWarning("User not found. UserId: {UserId}", request.UserId);
                    return ApplicationEmailErrors.UserNotFound(request.UserId);
                }

                // Check if email already exists
                var isAlreadyExists = user.Emails.Any(e => e.EmailAddress == request.EmailAddress);
                if (isAlreadyExists)
                {
                    _logger.LogWarning(
                        "Email already exists. Email: {Email}, UserId: {UserId}",
                        request.EmailAddress, request.UserId);
                    return ApplicationEmailErrors.EmailAlreadyExists(request.EmailAddress);
                }

                // Add email to user
                var createEmailResult = user.AddEmail(request.EmailAddress, request.Label, request.IsPrimary);
                if (createEmailResult.IsError)
                {
                    _logger.LogWarning(
                        "Failed to add email. Email: {Email}, Errors: {Errors}",
                        request.EmailAddress, string.Join(", ", createEmailResult.Errors));
                    return createEmailResult.Errors;
                }

                // Save changes
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Email added successfully. Email: {Email}, UserId: {UserId}",
                    request.EmailAddress, request.UserId);

                return createEmailResult.Value.ToDto();
        }
    }
}