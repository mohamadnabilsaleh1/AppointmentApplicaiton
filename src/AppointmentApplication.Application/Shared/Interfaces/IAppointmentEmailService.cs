using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Appointments;

namespace AppointmentApplication.Application.Shared.Interfaces
{
    public interface IAppointmentEmailService
    {
        Task SendAppointmentCreatedEmailAsync(Appointment appointment);
        Task SendAppointmentConfirmedEmailAsync(Appointment appointment);
        Task SendAppointmentCompletedEmailAsync(Appointment appointment);
        Task SendAppointmentCancelledEmailAsync(Appointment appointment, string cancellationReason);
    }
}