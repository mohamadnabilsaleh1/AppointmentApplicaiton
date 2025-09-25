using FluentValidation;
using System.Text.RegularExpressions;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.UpdateHealthcareFacility;

public sealed class UpdateHealthcareFacilityCommandValidator : AbstractValidator<UpdateHealthcareFacilityCommand>
{
    public UpdateHealthcareFacilityCommandValidator()
    {
        RuleFor(x => x.FacilityId)
            .NotEmpty().WithMessage("Facility ID is required");

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
    }
}