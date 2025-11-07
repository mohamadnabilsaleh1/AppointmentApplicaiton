using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Notifications.Dtos;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Notifications.Queries.GetUserNotifications
{
    public class GetUserNotificationsQuery : IRequest<Result<PaginationResult<NotificationDto>>>
    {
        public Guid UserId { get; }
        public bool? UnreadOnly { get; }
        public int Page { get; }
        public int PageSize { get; }

        public GetUserNotificationsQuery(Guid userId, bool? unreadOnly = false, int page = 1, int pageSize = 20)
        {
            UserId = userId;
            UnreadOnly = unreadOnly;
            Page = page;
            PageSize = pageSize;
        }
    }
}