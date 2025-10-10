using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Patients.Commands.AddAllergy;

using FluentValidation;

namespace AppointmentApplication.Application.Features.Patients.ChronicDiseases.Commands.DeleteChronicDisease
{
    public class DeleteChronicDiseaseCommandValidator : AbstractValidator<AddChronicDiseaseCommand>
    {
        public DeleteChronicDiseaseCommandValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.ChronicDiseaseType)
                .IsInEnum().WithMessage("{PropertyName} is not valid.");
        }
    }
}