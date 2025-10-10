using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

namespace AppointmentApplication.Application.Features.Patients.Commands.AddAllergy
{
    public class AddAllergyCommandValidator:AbstractValidator<AddAllergyCommand>
    {
        public AddAllergyCommandValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.AllergyType)
                .IsInEnum().WithMessage("{PropertyName} is not valid.");
        }
    }
}