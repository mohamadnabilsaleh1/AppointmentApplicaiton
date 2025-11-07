using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Notifications.Queries.GetUnreadNotificationsCount
{
    public class GetUnreadNotificationsCountQuery:IRequest<Result<int>>
    {
        public Guid UserId { get; }

        public GetUnreadNotificationsCountQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}