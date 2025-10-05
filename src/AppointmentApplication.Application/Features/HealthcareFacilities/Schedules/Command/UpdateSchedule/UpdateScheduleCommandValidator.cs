// 📁 Application/HealthcareFacilities/Schedules/Commands/UpdateScheduleCommandValidator.cs
using AppointmentApplication.Application.HealthcareFacilities.Schedules.Commands;
using FluentValidation;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Commands;

public sealed class UpdateScheduleCommandValidator : AbstractValidator<UpdateScheduleCommand>
{
    public UpdateScheduleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId must not be empty.");

        RuleFor(x => x.ScheduleId)
            .NotEmpty().WithMessage("ScheduleId must not be empty.");

        RuleFor(x => x.DayOfWeek)
            .IsInEnum().WithMessage("Invalid day of the week.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid schedule status.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required.")
            .GreaterThan(x => x.StartTime).WithMessage("End time must be greater than start time.");

        RuleFor(x => x)
            .Must(x => (x.EndTime - x.StartTime) <= TimeSpan.FromHours(24))
            .WithMessage("Duration cannot exceed 24 hours.");

        RuleFor(x => x.Note)
            .MaximumLength(500)
            .WithMessage("Note cannot exceed 500 characters.");
    }
}