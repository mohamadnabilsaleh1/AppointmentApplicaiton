using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.AddDescription
{
    public class AddDescriptionHealthCareFacilityCommand : IRequest<Result<Updated>>
    {
        public Guid UserId { get; }
        public string Description { get; }

        public AddDescriptionHealthCareFacilityCommand(Guid userId, string description)
        {
            UserId = userId;
            Description = description;
        }
    }
}