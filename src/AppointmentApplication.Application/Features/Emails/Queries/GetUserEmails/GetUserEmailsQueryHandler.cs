using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Emails.Dtos;
using AppointmentApplication.Application.Features.Emails.Errors;
using AppointmentApplication.Application.Features.Emails.Mapper;

using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Emails.Queries.GetUserEmails
{
    public class GetUserEmailsQueryHandler(
           ILogger<GetUserEmailsQueryHandler> logger,
           IAppDbContext context)
           : IRequestHandler<GetUserEmailsQuery, Result<List<EmailDto>>>
    {
        private readonly ILogger<GetUserEmailsQueryHandler> _logger = logger;
        private readonly IAppDbContext _context = context;

        public async Task<Result<List<EmailDto>>> Handle(GetUserEmailsQuery request, CancellationToken cancellationToken)
        {
 
                // Check if user exists
                var userExists = await _context.Users
                    .AnyAsync(u => u.Id == request.UserId, cancellationToken);

                if (!userExists)
                {
                    _logger.LogWarning("User not found. UserId: {UserId}", request.UserId);
                    return ApplicationEmailErrors.UserNotFound(request.UserId);
                }

                // Get user emails
                var emails = await _context.Emails
                    .Where(e => e.UserId == request.UserId)
                    .OrderByDescending(e => e.IsPrimary)
                    .ThenBy(e => e.Label)
                    .ToListAsync(cancellationToken);

                _logger.LogInformation("Retrieved {Count} emails for UserId: {UserId}", emails.Count, request.UserId);

                return emails.Select(e => e.ToDto()).ToList();

        }
    }
}