using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities.Departments;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.HealthcareFacilities.Schedules;
using AppointmentApplication.Domain.HealthcareFacilities.ScheduleExceptions;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Mappers;

public static class HealthcareFacilityMapper
{
    // ✅ Single entity to DTO
    public static HealthcareFacilityDto ToDto(this HealthCareFacility entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new HealthcareFacilityDto(
            entity.Id,
            entity.UserId,
            entity.Name,
            entity.Type,
            entity.Address.ToDto(),
            entity.GPSLatitude,
            entity.GPSLongitude,
            entity.IsActive,
            entity.CreatedAtUtc,
            entity.CreatedBy,
            entity.UpdatedAtdUtc,
            entity.LastModifiedBy,
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
    public static DepartmentDto ToDto(this Department entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new DepartmentDto(
            entity.Id,
            entity.FacilityId,
            entity.Name,
            entity.Description,
            entity.IsActive,
            entity.CreatedAtUtc);
    }

    public static List<DepartmentDto> ToDtos(this IEnumerable<Department> entities)
    {
        return entities.Select(e => e.ToDto()).ToList();
    }

    // ✅ Schedule mapping
    public static ScheduleDto ToDto(this ScheduleHealthcareFacility entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new ScheduleDto(
            entity.Id,
            entity.FacilityId,   // now consistent
            entity.DayOfWeek,
            entity.StartTime,
            entity.EndTime,
            entity.Status.ToString(),
            entity.IsAvailable,
            entity.Note);
    }
    public static List<ScheduleDto> ToDtos(this IEnumerable<ScheduleHealthcareFacility> entities)
    {
        return entities.Select(e => e.ToDto()).ToList();
    }

    // ✅ Schedule exception mapping
    public static ScheduleExceptionDto ToDto(this ScheduleExceptionHealthcareFacility entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new ScheduleExceptionDto(
            entity.Id,
            entity.FacilityId,   // now consistent
            entity.Date,
            entity.DayOfWeek,
            entity.StartTime,
            entity.EndTime,
            entity.Status.ToString(),
            entity.Reason);
    }

    public static List<ScheduleExceptionDto> ToDtos(this IEnumerable<ScheduleExceptionHealthcareFacility> entities)
    {
        return entities.Select(e => e.ToDto()).ToList();
    }
}
