// AppointmentApplication.Application/Features/MedicalRecords/Queries/GetMedicalRecordsForDoctorByPatientId/GetMedicalRecordsForDoctorByPatientIdQueryHandler.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppointmentApplication.Application.Features.MedicalRecords.Dtos;
using AppointmentApplication.Application.Features.Doctors.Errors;
using AppointmentApplication.Application.Features.Patients.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Utilities;
using AppointmentApplication.Domain.MedicalRecords;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.MedicalRecords.Queries.GetMedicalRecordsForDoctorByPatientId
{
    public class GetMedicalRecordsForDoctorByPatientIdQueryHandler
        : IRequestHandler<GetMedicalRecordsForDoctorByPatientIdQuery, Result<MedicalRecordForDoctorDto>>
    {
        private readonly IAppDbContext _context;

        public GetMedicalRecordsForDoctorByPatientIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<MedicalRecordForDoctorDto>> Handle(
            GetMedicalRecordsForDoctorByPatientIdQuery request,
            CancellationToken cancellationToken)
        {
            // Verify doctor exists and get doctor ID
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);

            if (doctor is null)
            {
                return ApplicationDoctorErrors.DoctorNotFound(request.UserId);
            }

            // Verify patient exists with allergies and chronic diseases
            var patient = await _context.Patients
                .Include(p => p.Allergies)
                .Include(p => p.ChronicDiseases)
                .FirstOrDefaultAsync(p => p.Id == request.PatientId, cancellationToken);

            if (patient is null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.PatientId);
            }

            // Get medical records for this patient that were created by this doctor
            var medicalRecords = await _context.MedicalRecords
                .Include(mr => mr.Patient)
                    .ThenInclude(p => p.Allergies)
                .Include(mr => mr.Patient)
                    .ThenInclude(p => p.ChronicDiseases)
                .Include(mr => mr.Doctor)
                .Include(mr => mr.Facility)
                    .ThenInclude(f => f.Address)
                .Include(mr => mr.Appointment)
                    .ThenInclude(a => a.Prescriptions)
                .Where(mr => mr.PatientId == request.PatientId && mr.DoctorId == doctor.Id)
                .OrderByDescending(mr => mr.RecordDate)
                .ToListAsync(cancellationToken);

            // Map allergies to DTO - using your enum-based structure
            var allergyDtos = patient.Allergies?
                .Select(a => new AllergyDto(
                    Id: a.Id,
                    Name: a.Name.ToString(), // Convert enum to string
                    AllergyType: a.Name
                )).ToList() ?? new List<AllergyDto>();

            // Map chronic diseases to DTO - using your enum-based structure
            var chronicDiseaseDtos = patient.ChronicDiseases?
                .Select(cd => new ChronicDiseaseDto(
                    Id: cd.Id,
                    Name: cd.Name.ToString(), // Convert enum to string
                    ChronicDiseaseType: cd.Name
                )).ToList() ?? new List<ChronicDiseaseDto>();

            // Map to DTO
            var result = new MedicalRecordForDoctorDto(
                Id: patient.Id,
                PatientFullName: $"{patient.FirstName} {patient.LastName}",
                PatientNationalId: patient.NationalID,
                PatientGender: patient.Gender.ToString(),
                PatientAge: AgeCalculator.CalculateAge(patient.DateOfBirth),
                Allergies: allergyDtos,
                ChronicDiseases: chronicDiseaseDtos,
                MedicalRecords: medicalRecords.Select(mr => new MedicalRecordItemForDoctorDto(
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
                    ),
                    Facility: new Dtos.FacilityInfoDto(
                        Id: mr.Facility.Id,
                        Name: mr.Facility.Name,
                        Address: $"{mr.Facility.Address.Street}, {mr.Facility.Address.City}, {mr.Facility.Address.State}, {mr.Facility.Address.Country}"
                    )
                )).ToList()
            );

            return result;
        }
    }
}