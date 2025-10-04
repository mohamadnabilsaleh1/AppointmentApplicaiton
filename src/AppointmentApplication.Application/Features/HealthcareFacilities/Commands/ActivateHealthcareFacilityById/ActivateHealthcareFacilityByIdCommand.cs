using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.ActivateHealthcareFacilityById;

public record ActivateHealthcareFacilityByIdCommand(Guid FacilityId) : IRequest<Result<Updated>>;
