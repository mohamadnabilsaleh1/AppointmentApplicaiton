using System;

using FluentValidation;

namespace AppointmentApplication.Application.Features.Appointments.Commands.RescheduleAppointment
{
    public class RescheduleAppointmentByPatientIdCommandValidator : AbstractValidator<RescheduleAppointmentByPatientIdCommand>
    {
        public RescheduleAppointmentByPatientIdCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.")
                .NotEqual(Guid.Empty).WithMessage("User ID must be a valid GUID.");

            RuleFor(x => x.AppointmentId)
                .NotEmpty().WithMessage("Appointment ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Appointment ID must be a valid GUID.");

            RuleFor(x => x.NewDate)
                .NotEmpty().WithMessage("New date is required.");

            RuleFor(x => x.NewTime)
                .Must(t => t >= TimeSpan.Zero && t < TimeSpan.FromHours(24))
                .WithMessage("New time must be within a valid 24-hour window.");
        }
    }
}
