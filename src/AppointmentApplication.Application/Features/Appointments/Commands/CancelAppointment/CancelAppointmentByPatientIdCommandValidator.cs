using System;

using FluentValidation;

namespace AppointmentApplication.Application.Features.Appointments.Commands.CancelAppointment
{
    public class CancelAppointmentByPatientIdCommandValidator : AbstractValidator<CancelAppointmentByPatientIdCommand>
    {
        public CancelAppointmentByPatientIdCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Patient ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Patient ID must be a valid GUID.");

            RuleFor(x => x.AppointmentId)
                .NotEmpty().WithMessage("Appointment ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Appointment ID must be a valid GUID.");

            RuleFor(x => x.CancellationReason)
                .NotEmpty().WithMessage("Cancellation reason is required.")
                .MaximumLength(500).WithMessage("Cancellation reason cannot exceed 500 characters.");
        }
    }
}
