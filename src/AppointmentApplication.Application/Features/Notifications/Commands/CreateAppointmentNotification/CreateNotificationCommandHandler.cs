using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Notifications;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Notifications.Commands.CreateAppointmentCommand
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateAppointmentNotificationCommand, Result<Success>>
    {
        private readonly IAppDbContext _context;
        private readonly ILogger<CreateNotificationCommandHandler> _logger;

        public CreateNotificationCommandHandler(IAppDbContext context, ILogger<CreateNotificationCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Success>> Handle(CreateAppointmentNotificationCommand request, CancellationToken cancellationToken)
        {
            var notificationResult = Notification.Create(
                request.UserId,
                request.Title,
                request.Message,
                request.Type,
                request.RelatedEntityId,
                request.RelatedEntityType);

            if (notificationResult.IsError)
                return notificationResult.Errors;

            _context.Notifications.Add(notificationResult.Value);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Notification created for user {UserId}", request.UserId);
            return Result.Success;
        }
    }
}