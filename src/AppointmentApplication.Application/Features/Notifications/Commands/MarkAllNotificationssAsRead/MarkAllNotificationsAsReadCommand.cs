using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Notifications.Commands.MarkAllNotificationssAsRead
{
public class MarkAllNotificationsAsReadCommand : IRequest<Result<Updated>>
{
    public Guid UserId { get; }

    public MarkAllNotificationsAsReadCommand(Guid userId)
    {
        UserId = userId;
    }
}
}