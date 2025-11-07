using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using AppointmentApplication.Application.Shared.Services;

namespace AppointmentApplication.Application.Features.Notifications.Commands.MarkAllNotificationssAsRead
{
    public class MarkAllNotificationsAsReadCommandHandler : IRequestHandler<MarkAllNotificationsAsReadCommand, Result<Updated>>
    {
        private readonly IAppDbContext _context;
        private readonly ILogger<MarkAllNotificationsAsReadCommandHandler> _logger;

        private readonly IHubContext<NotificationHub> _hubContext;

        public MarkAllNotificationsAsReadCommandHandler(
            IAppDbContext context,
            ILogger<MarkAllNotificationsAsReadCommandHandler> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task<Result<Updated>> Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            var unreadNotifications = await _context.Notifications
                .Where(n => n.UserId == request.UserId && !n.IsRead)
                .ToListAsync(cancellationToken);

            foreach (var notification in unreadNotifications)
            {
                notification.MarkAsRead();
            }

            if (unreadNotifications.Any())
            {
                await _context.SaveChangesAsync(cancellationToken);

                // Send real-time update to client
                await _hubContext.Clients.Group($"user-{request.UserId}")
                    .SendAsync("AllNotificationsRead");
            }

            _logger.LogInformation("All notifications marked as read for user {UserId}", request.UserId);
            return Result.Updated;
        }
    }
}