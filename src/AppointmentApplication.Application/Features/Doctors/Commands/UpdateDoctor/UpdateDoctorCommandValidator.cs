using System;

using FluentValidation;

namespace AppointmentApplication.Application.Features.Doctors.Commands.UpdateDoctor;

public class UpdateDoctorCommandValidator : AbstractValidator<UpdateDoctorCommand>
{
    public UpdateDoctorCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required")
            .NotEqual(Guid.Empty).WithMessage("User ID must be a valid GUID");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters")
            .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("First name can only contain letters, spaces, hyphens, and apostrophes");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters")
            .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("Last name can only contain letters, spaces, hyphens, and apostrophes");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Gender must be a valid value");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .Must(dob => dob < DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Date of birth must be in the past")
            .Must(BeAValidAge)
            .WithMessage("Doctor must be at least 18 years old");
    }

    private bool BeAValidAge(DateOnly dateOfBirth)
    {
        const int minimumAge = 18;
        var today = DateOnly.FromDateTime(DateTime.Today);

        int age = today.Year - dateOfBirth.Year;

        // If the birthday hasn't occurred this year yet, subtract 1
        if (dateOfBirth > today.AddYears(-age))
        {
            age--;
        }

        return age >= minimumAge;
    }
}