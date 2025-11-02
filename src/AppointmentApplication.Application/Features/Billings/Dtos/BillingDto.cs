using System;

using AppointmentApplication.Domain.Billings.Enums;

namespace AppointmentApplication.Application.Features.Billings.Dtos
{
    public sealed record BillingDto(
        Guid Id,
        Guid AppointmentId,
        Guid PatientId,
        Guid DoctorId,
        DateTime DateIssued,
        decimal TotalAmount,
        BillingStatus Status,
        DateTime? PaymentDate,
        decimal? PaidAmount,
        PatientInfoDto Patient,        // تأكد أن هذا من نفس الـ namespace
        DoctorInfoDto Doctor,          // تأكد أن هذا من نفس الـ namespace
        AppointmentInfoDto Appointment // تأكد أن هذا من نفس الـ namespace
    );

    public record PatientInfoDto(
        Guid Id,
        string FullName,
        string NationalID);

    public record DoctorInfoDto(
        Guid Id,
        string FullName,
        string Specialization);

    public record AppointmentInfoDto(
        Guid Id,
        DateOnly ScheduledDate,
        TimeSpan ScheduledTime,
        string Status);
}