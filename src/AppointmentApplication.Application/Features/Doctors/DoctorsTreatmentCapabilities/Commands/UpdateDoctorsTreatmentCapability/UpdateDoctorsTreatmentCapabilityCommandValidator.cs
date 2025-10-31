using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Commands.UpdateDoctorsTreatmentCapability;
using FluentValidation;

namespace AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Validators
{
    public class UpdateDoctorsTreatmentCapabilityCommandValidator : AbstractValidator<UpdateDoctorsTreatmentCapabilityCommand>
    {
        public UpdateDoctorsTreatmentCapabilityCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Doctor ID is required")
                .NotEqual(Guid.Empty).WithMessage("Doctor ID must be a valid GUID");

            RuleFor(x => x.MaxPatientsPerDay)
                .GreaterThan(0).WithMessage("Maximum patients per day must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("Maximum patients per day cannot exceed 100")
                .WithName("Max Patients Per Day");

            RuleFor(x => x.SessionDurationMinutes)
                .GreaterThan(0).WithMessage("Session duration must be greater than 0 minutes")
                .LessThanOrEqualTo(480).WithMessage("Session duration cannot exceed 8 hours (480 minutes)")
                .Must(BeInValidIntervals).WithMessage("Session duration should be in 15-minute intervals (15, 30, 45, 60, etc.)")
                .WithName("Session Duration");
        }

        private bool BeInValidIntervals(int sessionDurationMinutes)
        {
            return sessionDurationMinutes % 15 == 0;
        }
    }
}