using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.UpdateHealthcareFacilityByUserId
{
    public record UpdateHealthcareFacilityByUserIdCommand(
    Guid UserId,
    string Name,
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode,
    double GPSLatitude,
    double GPSLongitude) : IRequest<Result<Updated>>;
}