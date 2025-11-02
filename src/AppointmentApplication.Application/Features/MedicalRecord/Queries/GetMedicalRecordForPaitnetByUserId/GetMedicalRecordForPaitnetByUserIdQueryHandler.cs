// AppointmentApplication.Application/Features/MedicalRecords/Queries/GetAllMedicalRecords/GetAllMedicalRecordsQueryHandler.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.MedicalRecords.Dtos;
using AppointmentApplication.Application.Features.Patients.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.MedicalRecords;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.MedicalRecords.Queries.GetAllMedicalRecords
{
    public class GetMedicalRecordForPaitnetByUserIdQueryHandler
        : IRequestHandler<GetMedicalRecordForPaitnetByUserIdQuery, Result<List<MedicalRecordDto>>>
    {
        private readonly IAppDbContext _context;

        public GetMedicalRecordForPaitnetByUserIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<MedicalRecordDto>>> Handle(
            GetMedicalRecordForPaitnetByUserIdQuery request,
            CancellationToken cancellationToken)
        {
            // Find patient by user ID
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

            if (patient is null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }

            // Get all medical records with related data
            var medicalRecords = await _context.MedicalRecords
                .Include(mr => mr.Patient)
                .Include(mr => mr.Doctor)
                .Include(mr => mr.Facility)
                .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Prescriptions)
                .Where(mr => mr.PatientId == patient.Id)
                .OrderByDescending(mr => mr.RecordDate)
                .ToListAsync(cancellationToken);

            // Group by patient and map to DTO
            var result = medicalRecords
                .GroupBy(mr => mr.PatientId)
                .Select(group => new MedicalRecordDto(
                    Id: group.Key,
                    PatientFullName: $"{group.First().Patient.FirstName} {group.First().Patient.LastName}",
                    MedicalRecord: group.Select(mr => new MedicalRecordItemDto(
                        DoctorFullName: $"{mr.Doctor.FirstName} {mr.Doctor.LastName}",
                        FacilityName: mr.Facility.Name,
                        RecordDate: mr.RecordDate,
                        Diagnosis: mr.Diagnosis,
                        TreatmentNotes: mr.TreatmentNotes,
                        FollowUpInstructions: mr.FollowUpInstructions,
                        Prescriptions: mr.Appointment.Prescriptions?
                            .Select(p => new PrescriptionMedicalInfoDto(
                                Id: p.Id,
                                DateIssued: p.DateIssued,
                                MedicationList: p.MedicationList,
                                DosageInstructions: p.DosageInstructions
                            )).ToList() ?? new List<PrescriptionMedicalInfoDto>(),
                        Appointment: new AppointmentInfoDto(
                            Id: mr.Appointment.Id,
                            ScheduledDate: mr.Appointment.ScheduledDate,
                            ScheduledTime: mr.Appointment.ScheduledTime,
                            Status: mr.Appointment.Status.ToString(),
                            Notes: mr.Appointment.Notes ?? string.Empty
                        )
                    )).ToList()
                ))
                .ToList();

            return result;
        }
    }
}