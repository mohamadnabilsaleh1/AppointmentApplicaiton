using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.DeactivateHealthcareFacilityById;

using FluentValidation;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands;

public class DeactivateHealthcareFacilityByIdCommandValidator
    : AbstractValidator<DeactivateHealthcareFacilityByIdCommand>
{
    public DeactivateHealthcareFacilityByIdCommandValidator()
    {
        RuleFor(x => x.FacilityId)
            .NotEmpty().WithMessage("FacilityId must not be empty.");
    }
}