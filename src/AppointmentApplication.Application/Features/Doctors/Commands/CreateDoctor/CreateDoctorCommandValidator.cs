using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

namespace AppointmentApplication.Application.Features.Doctors.Commands.CreateDoctor
{
    public class CreateDoctorCommandValidator : AbstractValidator<CreateDoctorCommand>
    {
        public CreateDoctorCommandValidator()
        {

        }
    }
}