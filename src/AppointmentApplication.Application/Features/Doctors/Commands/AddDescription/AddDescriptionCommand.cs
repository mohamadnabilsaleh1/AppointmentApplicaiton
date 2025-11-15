using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.Commands.AddDescription
{
 public class AddDescriptionCommand : IRequest<Result<Updated>>
    {
        public Guid UserId { get; }
        public string Description { get; }

        public AddDescriptionCommand(Guid userId, string description)
        {
            UserId = userId;
            Description = description;
        }
    }
}