using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.ActivateHealthcareFacilityById;

using FluentValidation;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands;

public class ActivateHealthcareFacilityByIdCommandValidator
    : AbstractValidator<ActivateHealthcareFacilityByIdCommand>
{
    public ActivateHealthcareFacilityByIdCommandValidator()
    {
        RuleFor(x => x.FacilityId)
            .NotEmpty().WithMessage("FacilityId must not be empty.");
    }
}