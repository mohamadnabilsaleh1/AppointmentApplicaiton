using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

namespace AppointmentApplication.Application.Features.Patients.Uploads.Commands.ChangeFileToPrivate
{
    public class ChangeFileToPrivateCommandValidator : AbstractValidator<ChangeFileToPrivateCommand>
    {
        public ChangeFileToPrivateCommandValidator()
        {
            RuleFor(c => c.UploadId).NotEmpty().WithMessage("UploadId is required.");
        }
    }
}