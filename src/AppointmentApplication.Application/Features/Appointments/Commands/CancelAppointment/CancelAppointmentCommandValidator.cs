using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Appointments.Commands.ConfirmAppointment;

using FluentValidation;

namespace AppointmentApplication.Application.Features.Appointments.Commands.CancelAppointment
{
    public class CancelAppointmentCommandValidator : AbstractValidator<ConfirmAppointmentCommand>
    {
        public CancelAppointmentCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.")
                .NotEqual(Guid.Empty).WithMessage("User ID must be a valid GUID.");

            RuleFor(x => x.AppointmentId)
                .NotEmpty().WithMessage("Appointment ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Appointment ID must be a valid GUID.");
        }
    }
}