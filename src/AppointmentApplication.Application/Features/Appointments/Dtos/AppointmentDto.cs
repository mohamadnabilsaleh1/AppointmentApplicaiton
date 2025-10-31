using System;
using System.Collections.Generic;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Appointments.Enums;
// using AppointmentApplication.Application.Features.Billings.Dtos;

namespace AppointmentApplication.Application.Features.Appointments.Dtos
{
    public sealed record AppointmentDto(
        Guid Id,
        Guid PatientId,
        Guid DoctorId,
        Guid FacilityId,
        DateOnly ScheduledDate,
        TimeSpan ScheduledTime,
        int DurationMinutes,
        AppointmentStatus Status,
        DateTime BookingDate,
        DateTime? CheckInTime,
        DateTime? CheckOutTime,
        string Notes,
        string CancellationReason,
        PatientDto Patient,
        DoctorDto Doctor,
        FacilityDto Facility
        // BillingDto? Billing,
        // IReadOnlyCollection<PrescriptionDto> Prescriptions
    );

    public sealed record PatientDto(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber
    );

    public sealed record DoctorDto(
        Guid Id,
        string FirstName,
        string LastName,
        string Specialization
    );

    public sealed record FacilityDto(
        Guid Id,
        string Name,
        string City
    );
}