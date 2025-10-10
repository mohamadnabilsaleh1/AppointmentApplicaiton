using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

namespace AppointmentApplication.Application.Features.Patients.Commands.UpdatePatient
{
    public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
    {
        public UpdatePatientCommandValidator()
        {
            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("National ID must be greater than 0.");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Date of Birth must be in the past.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required");
        }
    }
}