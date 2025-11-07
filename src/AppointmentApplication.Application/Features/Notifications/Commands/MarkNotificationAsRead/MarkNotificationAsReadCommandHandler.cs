using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Notifications.Errors;

using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Services;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.AspNetCore.SignalR;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, Result<Updated>>
    {
        private readonly IAppDbContext _context;
        private readonly ILogger<MarkNotificationAsReadCommandHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public MarkNotificationAsReadCommandHandler(
            IAppDbContext context,
            ILogger<MarkNotificationAsReadCommandHandler> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task<Result<Updated>> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == request.NotificationId && n.UserId == request.UserId, cancellationToken);

            if (notification == null)
            {
                return ApplicationNotificationErrors.NotificationNotFound(request.NotificationId);
            }

            notification.MarkAsRead();
            await _context.SaveChangesAsync(cancellationToken);

            // Send real-time update to client
            await _hubContext.Clients.Group($"user-{request.UserId}")
                .SendAsync("NotificationRead", new { NotificationId = notification.Id });

            _logger.LogInformation("Notification {NotificationId} marked as read by user {UserId}",
                request.NotificationId, request.UserId);

            return Result.Updated;
        }
    }
}