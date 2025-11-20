using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Mappers;
using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Mappers;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Mappers;
using AppointmentApplication.Application.Features.Users.Mappers;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities.Departments;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.HealthcareFacilities.ScheduleExceptions;
using AppointmentApplication.Domain.HealthcareFacilities.Schedules;

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Mappers;

public static class HealthcareFacilityMapper
{
    // ✅ Single entity to DTO
    public static HealthcareFacilityDto ToDto(this HealthCareFacility entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new HealthcareFacilityDto(
            entity.Id,
            entity.Name,
            entity.Type,
            entity.Address.ToDto(),
            entity.GPSLatitude,
            entity.GPSLongitude,
            entity.Description,
            entity.User.Avatar == string.Empty ? "" : "api/users/" + entity.User.Id.ToString() + "/avatar",
            entity.Departments.ToDtos(),
            entity.Schedules.ToDtos(),
            entity.ScheduleExceptions.ToDtos());
    }

    public static HealthcareFacilityWithUserDto ToDtoWithUser(this HealthCareFacility entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new HealthcareFacilityWithUserDto(
            entity.Id,
            entity.Name,
            entity.Type,
            entity.Address.ToDto(),
            entity.GPSLatitude,
            entity.GPSLongitude,
            entity.User is null ? string.Empty : entity.User.Email,
            entity.Description,
            entity.User.Avatar == string.Empty ? "" : "api/users/" + entity.User.Id.ToString() + "/avatar",
            entity.Departments.ToDtos(),
            entity.Schedules.ToDtos(),
            entity.ScheduleExceptions.ToDtos());
    }

    // ✅ Collection to DTOs
    public static List<HealthcareFacilityDto> ToDtos(this IEnumerable<HealthCareFacility> entities)
    {
        return entities.Select(e => e.ToDto()).ToList();
    }

    // ✅ Address mapping
    public static AddressDto ToDto(this Address entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new AddressDto(
            entity.Street,
            entity.City,
            entity.Country,
            entity.ZipCode);
    }

    // ✅ Department mapping

}
