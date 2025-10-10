using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

namespace AppointmentApplication.Application.Features.Patients.Commands.AddAllergy
{
    public class AddChronicDiseaseCommandValidator:AbstractValidator<AddChronicDiseaseCommand>
    {
        public AddChronicDiseaseCommandValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.ChronicDiseaseType)
                .IsInEnum().WithMessage("{PropertyName} is not valid.");
        }
    }
}