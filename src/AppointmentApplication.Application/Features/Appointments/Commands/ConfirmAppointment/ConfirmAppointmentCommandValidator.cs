using System;
using FluentValidation;

namespace AppointmentApplication.Application.Features.Appointments.Commands.ConfirmAppointment
{
    public class ConfirmAppointmentCommandValidator : AbstractValidator<ConfirmAppointmentCommand>
    {
        public ConfirmAppointmentCommandValidator()
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