using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

/*
    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public Email Email { get; private set; }
*/
public sealed record CreateHealthcareFacilityCommand(string FirstName, string LastName, string Email, string Password, string Name, HealthCareType Type, string Street, string City, string State, string Country, string ZipCode, double GPSLatitude, double GPSLongitude) : IRequest<Result<HealthCareFacility>>;
