using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Notifications.Dtos;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Notifications.Queries.GetUserNotifications
{
    public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, Result<PaginationResult<NotificationDto>>>
    {
        private readonly IAppDbContext _context;
        private readonly ILogger<GetUserNotificationsQueryHandler> _logger;

        public GetUserNotificationsQueryHandler(IAppDbContext context, ILogger<GetUserNotificationsQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<PaginationResult<NotificationDto>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Filter to only include unread notifications (IsRead == false)
                var query = _context.Notifications
                    .Where(n => n.UserId == request.UserId && !n.IsRead) // Only unread notifications
                    .OrderByDescending(n => n.CreatedAtUtc)
                    .AsQueryable();

                // استخدام الـ PaginationResult الجديد
                var result = await PaginationResult<NotificationDto>.CreateAsync(
                    query.Select(n => new NotificationDto
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Message = n.Message,
                        Type = n.Type,
                        IsRead = n.IsRead,
                        ReadAt = n.ReadAt,
                        RelatedEntityId = n.RelatedEntityId,
                        RelatedEntityType = n.RelatedEntityType,
                        CreatedAtUtc = n.CreatedAtUtc
                    }),
                    request.Page,
                    request.PageSize);

                _logger.LogInformation(
                    "Retrieved {Count} unread notifications for user {UserId}, Page: {Page}, PageSize: {PageSize}",
                    result.Items.Count, request.UserId, request.Page, request.PageSize);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving unread notifications for user {UserId}", request.UserId);
                throw;
            }
        }
    }
}
