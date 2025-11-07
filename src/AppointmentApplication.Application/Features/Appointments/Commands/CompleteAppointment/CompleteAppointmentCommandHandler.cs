using System;
using System.Threading;
using System.Threading.Tasks;
using AppointmentApplication.Application.Features.Appointments.Commands.CompleteAppointment;
using AppointmentApplication.Application.Features.Appointments.Dtos;
using AppointmentApplication.Application.Features.Appointments.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Appointments.Enums;
using AppointmentApplication.Domain.Billings;
using AppointmentApplication.Domain.Billings.Enums;
using AppointmentApplication.Domain.MedicalRecords;
using AppointmentApplication.Domain.Prescriptions;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Appointments.Commands.CompleteAppointment
{
    public class CompleteAppointmentCommandHandler : IRequestHandler<CompleteAppointmentCommand, Result<AppointmentCompletionDto>>
    {
        private readonly ILogger<CompleteAppointmentCommandHandler> _logger;
        private readonly IAppDbContext _context;
        private readonly IAppointmentEmailService _emailService;

        public CompleteAppointmentCommandHandler(
            ILogger<CompleteAppointmentCommandHandler> logger,
            IAppDbContext context,
            IAppointmentEmailService emailService)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
        }

        public async Task<Result<AppointmentCompletionDto>> Handle(
            CompleteAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Completing appointment {AppointmentId} for user {UserId}",
                request.AppointmentId, request.UserId);

            // 1. Find appointment with all related data
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                .Include(a => a.Facility)
                .Include(a => a.Billing)
                .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                _logger.LogWarning("Appointment not found: {AppointmentId}", request.AppointmentId);
                return ApplicationAppointmentErrors.AppointmentNotFound(request.AppointmentId);
            }

            // 2. Verify the user is the assigned doctor
            if (appointment.Doctor.UserId != request.UserId)
            {
                _logger.LogWarning(
                    "User {UserId} is not authorized to complete appointment {AppointmentId}",
                    request.UserId, request.AppointmentId);
                return ApplicationAppointmentErrors.UnauthorizedToCompleteAppointment(request.AppointmentId);
            }

            // 3. Validate appointment can be completed
            if (appointment.Status != AppointmentStatus.Confirmed)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} cannot be completed. Current status: {Status}",
                    request.AppointmentId, appointment.Status);
                return ApplicationAppointmentErrors.CannotCompleteAppointment(appointment.Status);
            }

            // 5. Complete appointment using domain method
            var completeResult = appointment.Complete();
            if (completeResult.IsError)
            {
                _logger.LogWarning(
                    "Failed to complete appointment: {Errors}",
                    string.Join(", ", completeResult.Errors));
                return completeResult.Errors;
            }

            // 6. Create medical record
            var medicalRecordResult = MedicalRecord.Create(
                appointment.PatientId,
                appointment.FacilityId,
                appointment.DoctorId,
                appointment.Id,
                request.Diagnosis,
                request.TreatmentNotes,
                request.FollowUpInstructions);

            if (medicalRecordResult.IsError)
            {
                _logger.LogWarning(
                    "Failed to create medical record: {Errors}",
                    string.Join(", ", medicalRecordResult.Errors));
                return medicalRecordResult.Errors;
            }

            var medicalRecord = medicalRecordResult.Value;
            _context.MedicalRecords.Add(medicalRecord);

            // 7. Create prescription
            var prescriptionResult = Prescription.Create(
                appointment.Id,
                appointment.DoctorId,
                request.MedicationList,
                request.DosageInstructions);

            if (prescriptionResult.IsError)
            {
                _logger.LogWarning(
                    "Failed to create prescription: {Errors}",
                    string.Join(", ", prescriptionResult.Errors));
                return prescriptionResult.Errors;
            }

            var prescription = prescriptionResult.Value;
            _context.Prescriptions.Add(prescription);

            // 8. Save all changes
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Appointment completed successfully. AppointmentId: {AppointmentId}, " +
                "MedicalRecordId: {MedicalRecordId}, PrescriptionId: {PrescriptionId}",
                appointment.Id, medicalRecord.Id, prescription.Id);

            // 9. Send completion email ASYNCHRONOUSLY
            _ = Task.Run(async () =>
            {
                try
                {
                    await _emailService.SendAppointmentCompletedEmailAsync(appointment);
                }
                catch (Exception emailEx)
                {
                    _logger.LogError(emailEx, "❌ Background email sending failed for appointment completion");
                }
            });

            // 10. Return completion result
            var result = new AppointmentCompletionDto(
                AppointmentId: appointment.Id,
                Status: appointment.Status.ToString(),
                MedicalRecordId: medicalRecord.Id,
                PrescriptionId: prescription.Id,
                BillingPaid: true);

            return result;
        }
    }
}