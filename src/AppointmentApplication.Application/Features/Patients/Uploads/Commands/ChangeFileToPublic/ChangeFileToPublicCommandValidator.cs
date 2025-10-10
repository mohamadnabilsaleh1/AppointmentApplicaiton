using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

namespace AppointmentApplication.Application.Features.Patients.Uploads.Commands.ChangeFileToPublic
{
    public class ChangeFileToPublicCommandValidator:AbstractValidator<ChangeFileToPublicCommand>
    {
        public ChangeFileToPublicCommandValidator()
        {
            RuleFor(c => c.UploadId).NotEmpty().WithMessage("UploadId is required.");
        }
    }
}