using System;
using System.Collections.Generic;
using AppointmentApplication.Application.Features.Appointments.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Appointments.Commands.CompleteAppointment
{
    public sealed record CompleteAppointmentCommand(
        Guid UserId,
        Guid AppointmentId,
        string Diagnosis,
        string TreatmentNotes,
        string FollowUpInstructions,
        string MedicationList,
        string DosageInstructions) : IRequest<Result<AppointmentCompletionDto>>;

    public record AttachmentRequest(
        string FileType,
        string FileUrl,
        string Title,
        string Description,
        string Visibility = "Private");

    public record AppointmentCompletionDto(
        Guid AppointmentId,
        string Status,
        Guid MedicalRecordId,
        Guid PrescriptionId,
        bool BillingPaid);
}