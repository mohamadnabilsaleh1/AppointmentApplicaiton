using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Application.Shared.Interfaces
{
    public interface INotificationService
    {
        Task NotifyAppointmentCreatedAsync(Guid doctorUserId, Guid appointmentId, string patientName, DateTime scheduledDate, TimeSpan scheduledTime);
        Task NotifyAppointmentCancelledAsync(Guid patientUserId, Guid appointmentId, string doctorName, string reason);
        Task NotifyAppointmentConfirmedAsync(Guid patientUserId, Guid appointmentId, string doctorName);
        Task NotifyAppointmentCompletedAsync(Guid patientUserId, Guid appointmentId, string doctorName);
    }
}