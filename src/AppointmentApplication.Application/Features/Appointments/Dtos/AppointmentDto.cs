using AppointmentApplication.Domain.Appointments.Enums;

namespace AppointmentApplication.Application.Features.Appointments.Dtos
{
    public sealed record AppointmentDto(
        Guid Id,
        DateOnly ScheduledDate,
        TimeSpan ScheduledTime,
        int DurationMinutes,
        AppointmentStatus Status,
        DateTime BookingDate,
        string Notes,
        
        // Simplified nested objects - only essential info
        PatientInfoDto Patient,
        DoctorInfoDto Doctor,
        FacilityInfoDto Facility
    );
}

// Simplified DTOs for related entities
public record PatientInfoDto(
    Guid Id,
    string FullName,
    string? NationalID
);

public record DoctorInfoDto(
    Guid Id,
    string FullName,
    string Specialization
);

public record FacilityInfoDto(
    Guid Id,
    string Name,
    string Address
);