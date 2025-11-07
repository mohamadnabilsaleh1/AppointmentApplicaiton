using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Notifications.Queries.GetUnreadNotificationsCount
{
    public class GetUnreadNotificationsCountQueryHandler : IRequestHandler<GetUnreadNotificationsCountQuery, Result<int>>
    {
        private readonly IAppDbContext _context;

        public GetUnreadNotificationsCountQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(GetUnreadNotificationsCountQuery request, CancellationToken cancellationToken)
        {
            var count = await _context.Notifications
                .CountAsync(n => n.UserId == request.UserId && !n.IsRead, cancellationToken);

            return count;
        }
    }
}