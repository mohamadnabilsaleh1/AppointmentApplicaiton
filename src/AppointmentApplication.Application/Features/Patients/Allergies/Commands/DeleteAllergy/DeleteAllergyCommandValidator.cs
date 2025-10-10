using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

namespace AppointmentApplication.Application.Features.Patients.Allergies.Commands.DeleteAllergy
{
    public class DeleteAllergyCommandValidator : AbstractValidator<DeleteAllergyCommand>
    {
        public DeleteAllergyCommandValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.AllergyType)
                .IsInEnum().WithMessage("{PropertyName} is not valid.");
        }
    }
}