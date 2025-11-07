using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Notifications.Commands.DeleteNotification
{
    public class DeleteNotificationCommand:IRequest<Result<Deleted>>
    {
        public Guid NotificationId { get; }
        public Guid UserId { get; }

        public DeleteNotificationCommand(Guid notificationId, Guid userId)
        {
            NotificationId = notificationId;
            UserId = userId;
        }
    }
}