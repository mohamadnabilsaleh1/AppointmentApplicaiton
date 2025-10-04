using System;

using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Commands.UpdateHealthcareFacility;

public sealed record UpdateHealthcareFacilityByIdCommand(
    Guid FacilityId,
    string Name,
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode,
    double GPSLatitude,
    double GPSLongitude) : IRequest<Result<Updated>>;