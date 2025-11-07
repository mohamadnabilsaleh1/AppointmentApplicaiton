using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Application.Features.Notifications.Errors
{
    public class ApplicationNotificationErrors
    {
        public static Error NotificationNotFound(Guid NotificationId) =>
            Error.NotFound(
                "HealthCareFacility.NotFound",
                $"Healthcare facility with ID '{NotificationId}' was not found.");
    }
}