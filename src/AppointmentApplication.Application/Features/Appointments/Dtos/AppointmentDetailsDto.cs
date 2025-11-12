// AppointmentApplication.Application/Features/Appointments/Dtos/AppointmentDetailsDto.cs
using AppointmentApplication.Domain.Appointments.Enums;
using AppointmentApplication.Domain.Billings;
using AppointmentApplication.Domain.Prescriptions;

namespace AppointmentApplication.Application.Features.Appointments.Dtos
{
    public sealed record AppointmentDetailsDto(
        Guid Id,
        DateOnly ScheduledDate,
        TimeSpan ScheduledTime,
        int DurationMinutes,
        AppointmentStatus Status,
        DateTime BookingDate,
        DateTime? CheckInTime,
        DateTime? CheckOutTime,
        string? Notes,
        string CancellationReason,

        // Patient details
        PatientDetailsDto Patient,

        // Doctor details
        DoctorDetailsDto Doctor,

        // Facility details
        FacilityDetailsDto Facility,

        // Conditional details - only for completed appointments
        BillingDetailsDto? Billing,
        List<PrescriptionDetailsDto>? Prescriptions
    );

    // Patient details DTO
    public record PatientDetailsDto(
        Guid Id,
        string FullName,
        string NationalID,
        string Gender,
        int Age
    );

    // Doctor details DTO
    public record DoctorDetailsDto(
        Guid Id,
        string FullName,
        string Gender,
        int Age,
        string LicenseNumber,
        string Specialization
    );

    // Facility details DTO
    public record FacilityDetailsDto(
        Guid Id,
        string Name,
        string Type,
        AddressDto Address,
        double GPSLatitude,
        double GPSLongitude
    );

    // Billing details DTO
    public record BillingDetailsDto(
        Guid Id,
        decimal TotalAmount,
        string Status,
        DateTime DateIssued,
        DateTime? PaymentDate,
        decimal? PaidAmount
    );

    // Prescription details DTO
    public record PrescriptionDetailsDto(
        Guid Id,
        DateTime DateIssued,
        string MedicationList,
        string DosageInstructions
    );

    public record AddressDto(
        string Street,
        string City,
        string State,
        string Country,
        string ZipCode
    );
}