using System;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.UpdateHealthcareFacility;

public sealed record UpdateHealthcareFacilityCommand(
    Guid FacilityId,
    string Name,
    HealthCareType Type,
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode,
    double GPSLatitude,
    double GPSLongitude) : IRequest<Result<HealthCareFacility>>;