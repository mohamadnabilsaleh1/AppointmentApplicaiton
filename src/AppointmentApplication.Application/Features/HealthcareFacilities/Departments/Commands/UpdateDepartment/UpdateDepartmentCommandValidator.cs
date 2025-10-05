using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
    {
        public UpdateDepartmentCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long")
                .Matches(@"^[a-zA-Z\s\-'\.]+$").WithMessage("Name contains invalid characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(200).WithMessage("Description cannot exceed 50 characters")
                .MinimumLength(15).WithMessage("Description must be at least 2 characters long")
                .Matches(@"^[a-zA-Z\s\-'\.]+$").WithMessage("Description contains invalid characters");
        }
    }
}