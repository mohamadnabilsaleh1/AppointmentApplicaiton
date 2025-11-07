using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadCommand : IRequest<Result<Updated>>
    {
        public Guid NotificationId { get; }
        public Guid UserId { get; }

        public MarkNotificationAsReadCommand(Guid notificationId, Guid userId)
        {
            NotificationId = notificationId;
            UserId = userId;
        }
    }
}