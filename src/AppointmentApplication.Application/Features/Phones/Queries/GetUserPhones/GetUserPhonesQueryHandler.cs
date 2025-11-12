using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Phones.Dtos;

using AppointmentApplication.Application.Features.Phones.Errors;
using AppointmentApplication.Application.Features.Phones.Mapper;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Phones.Queries.GetUserPhones
{
    public class GetUserPhonesQueryHandler(
          ILogger<GetUserPhonesQueryHandler> logger,
          IAppDbContext context)
          : IRequestHandler<GetUserPhonesQuery, Result<List<PhoneDto>>>
    {
        private readonly ILogger<GetUserPhonesQueryHandler> _logger = logger;
        private readonly IAppDbContext _context = context;

        public async Task<Result<List<PhoneDto>>> Handle(GetUserPhonesQuery request, CancellationToken cancellationToken)
        {
            // Check if user exists
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == request.UserId, cancellationToken);

            if (!userExists)
            {
                _logger.LogWarning("User not found. UserId: {UserId}", request.UserId);
                return ApplicationPhoneErrors.UserNotFound(request.UserId);
            }

            // Get user phones
            var phones = await _context.Phones
                .Where(p => p.UserId == request.UserId)
                .OrderByDescending(p => p.IsPrimary)
                .ThenBy(p => p.Label)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} phones for UserId: {UserId}", phones.Count, request.UserId);

            return phones.Select(p => p.ToDto()).ToList();
        }
    }
}