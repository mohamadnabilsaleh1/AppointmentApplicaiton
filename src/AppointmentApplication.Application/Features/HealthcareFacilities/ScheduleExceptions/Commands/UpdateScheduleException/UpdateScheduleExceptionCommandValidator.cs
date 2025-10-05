// 📁 Application/HealthcareFacilities/ScheduleExceptions/Commands/UpdateScheduleExceptionCommandValidator.cs
using AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Commands;
using FluentValidation;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Commands;

public sealed class UpdateScheduleExceptionCommandValidator : AbstractValidator<UpdateScheduleExceptionCommand>
{
    public UpdateScheduleExceptionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId must not be empty.");

        RuleFor(x => x.ExceptionId)
            .NotEmpty().WithMessage("ExceptionId must not be empty.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required.")
            .GreaterThan(x => x.StartTime).WithMessage("End time must be greater than start time.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status.");

        RuleFor(x => x)
            .Must(x => (x.EndTime - x.StartTime) <= TimeSpan.FromHours(24))
            .WithMessage("Duration cannot exceed 24 hours.");

        RuleFor(x => x.Reason)
            .MaximumLength(500)
            .WithMessage("Reason cannot exceed 500 characters.");
    }
}