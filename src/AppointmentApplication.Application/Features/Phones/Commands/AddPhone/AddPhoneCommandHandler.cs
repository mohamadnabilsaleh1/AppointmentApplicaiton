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

namespace AppointmentApplication.Application.Features.Phones.Commands.AddPhone
{
    public class AddPhoneCommandHandler(
        ILogger<AddPhoneCommandHandler> logger,
        IAppDbContext context)
        : IRequestHandler<AddPhoneCommand, Result<PhoneDto>>
    {
        private readonly ILogger<AddPhoneCommandHandler> _logger = logger;
        private readonly IAppDbContext _context = context;

        public async Task<Result<PhoneDto>> Handle(AddPhoneCommand request, CancellationToken cancellationToken)
        {
            // Get user with phones included
            var user = await _context.Users
                .Include(u => u.Phones)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                _logger.LogWarning("User not found. UserId: {UserId}", request.UserId);
                return ApplicationPhoneErrors.UserNotFound(request.UserId);
            }

            // Check if phone already exists
            var isAlreadyExists = user.Phones.Any(p => p.PhoneNumber == request.PhoneNumber);
            if (isAlreadyExists)
            {
                _logger.LogWarning(
                    "Phone already exists. Phone: {Phone}, UserId: {UserId}",
                    request.PhoneNumber, request.UserId);
                return ApplicationPhoneErrors.PhoneAlreadyExists(request.PhoneNumber);
            }

            // Add phone to user
            var createPhoneResult = user.AddPhone(request.PhoneNumber, request.Label, request.IsPrimary);
            if (createPhoneResult.IsError)
            {
                _logger.LogWarning(
                    "Failed to add phone. Phone: {Phone}, Errors: {Errors}",
                    request.PhoneNumber, string.Join(", ", createPhoneResult.Errors));
                return createPhoneResult.Errors;
            }

            // Save changes
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Phone added successfully. Phone: {Phone}, UserId: {UserId}",
                request.PhoneNumber, request.UserId);

            return createPhoneResult.Value.ToDto();
        }
    }
}