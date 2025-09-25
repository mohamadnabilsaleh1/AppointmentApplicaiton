using System.Text.RegularExpressions;

using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using FluentValidation;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

public sealed class CreateHealthcareFacilityCommandValidator : AbstractValidator<CreateHealthcareFacilityCommand>
{
    public CreateHealthcareFacilityCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters long")
            .Matches(@"^[a-zA-Z\s\-'\.]+$").WithMessage("First name contains invalid characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters long")
            .Matches(@"^[a-zA-Z\s\-'\.]+$").WithMessage("Last name contains invalid characters");

        // Email validation
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required")
            .MaximumLength(100).WithMessage("Email address cannot exceed 100 characters")
            .EmailAddress().WithMessage("Invalid email address format")
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage("Invalid email address format");

        // Password validation
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number")
            .Matches(@"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]")
            .WithMessage("Password must contain at least one special character")
            .Must(NotContainWhitespace).WithMessage("Password cannot contain whitespace");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Facility name is required")
            .MaximumLength(200).WithMessage("Facility name cannot exceed 200 characters")
            .MinimumLength(2).WithMessage("Facility name must be at least 2 characters long")
            .Matches(@"^[a-zA-Z0-9\s\.\-&',()]+$").WithMessage("Facility name contains invalid characters");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid healthcare facility type");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street address is required")
            .MaximumLength(150).WithMessage("Street address cannot exceed 150 characters")
            .Matches(@"^[a-zA-Z0-9\s\.\-,#]+$").WithMessage("Street address contains invalid characters");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(100).WithMessage("City cannot exceed 100 characters")
            .Matches(@"^[a-zA-Z\s\-]+$").WithMessage("City name contains invalid characters");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(100).WithMessage("Country cannot exceed 100 characters")
            .Matches(@"^[a-zA-Z\s\-]+$").WithMessage("Country name contains invalid characters");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("Zip code is required")
            .MaximumLength(20).WithMessage("Zip code cannot exceed 20 characters")
            .Matches(@"^[a-zA-Z0-9\s\-]+$").WithMessage("Zip code contains invalid characters");

        RuleFor(x => x.GPSLatitude)
            .InclusiveBetween(-90, 90).WithMessage("GPS latitude must be between -90 and 90 degrees");

        RuleFor(x => x.GPSLongitude)
            .InclusiveBetween(-180, 180).WithMessage("GPS longitude must be between -180 and 180 degrees");

        // Cross-property validation
        RuleFor(x => x)
            .Must(HaveValidAddress).WithMessage("Complete address information is required")
            .Must(HaveValidCoordinatePrecision).WithMessage("GPS coordinates must have reasonable precision");
    }

    private bool HaveValidAddress(CreateHealthcareFacilityCommand command)
    {
        return !string.IsNullOrWhiteSpace(command.Street) &&
               !string.IsNullOrWhiteSpace(command.City) &&
               !string.IsNullOrWhiteSpace(command.Country) &&
               !string.IsNullOrWhiteSpace(command.ZipCode);
    }

    private bool HaveValidCoordinatePrecision(CreateHealthcareFacilityCommand command)
    {
        // Prevent unrealistic precision (more than 6 decimal places)
        string latitudeString = command.GPSLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
        string longitudeString = command.GPSLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture);

        int latitudePrecision = latitudeString.Contains('.') ? latitudeString.Split('.')[1].Length : 0;
        int longitudePrecision = longitudeString.Contains('.') ? longitudeString.Split('.')[1].Length : 0;

        return latitudePrecision <= 6 && longitudePrecision <= 6;
    }

    private bool NotContainWhitespace(string password)
    {
        return !string.IsNullOrWhiteSpace(password) && !password.Any(char.IsWhiteSpace);
    }
}