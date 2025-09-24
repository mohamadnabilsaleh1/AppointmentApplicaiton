using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

public sealed record CreateHealthcareFacilityCommand(string UserId, string Name,HealthCareType Type, string Street, string City, string State,string Country, string ZipCode, double GPSLatitude, double GPSLongitude):IRequest<Result<HealthCareFacility>>;
