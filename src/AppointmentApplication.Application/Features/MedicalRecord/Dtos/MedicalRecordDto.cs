// AppointmentApplication.Application/Features/MedicalRecords/Dtos/MedicalRecordDtos.cs
using System;
using System.Collections.Generic;
using AppointmentApplication.Domain.Patients.Allergies.Enums;
using AppointmentApplication.Domain.Patients.ChronicDiseases.Enums;

namespace AppointmentApplication.Application.Features.MedicalRecords.Dtos
{
    // Main DTO as requested with medicalRecord as array
    public record MedicalRecordDto(
        Guid Id,
        string PatientFullName,
        List<AllergyDto> Allergies,
        List<ChronicDiseaseDto> ChronicDiseases,
        List<MedicalRecordItemDto> MedicalRecord
    );

    public record MedicalRecordItemDto(
        string DoctorFullName,
        string FacilityName,
        DateTime RecordDate,
        string Diagnosis,
        string TreatmentNotes,
        string FollowUpInstructions,
        List<PrescriptionMedicalInfoDto> Prescriptions,
        AppointmentInfoDto Appointment
    );

    public record MedicalRecordForDoctorDto(
        Guid Id,
        string PatientFullName,
        string PatientNationalId,
        string PatientGender,
        int PatientAge,
        List<AllergyDto> Allergies,
        List<ChronicDiseaseDto> ChronicDiseases,
        List<MedicalRecordItemForDoctorDto> MedicalRecords
    );

    public record MedicalRecordItemForDoctorDto(
        DateTime RecordDate,
        string Diagnosis,
        string TreatmentNotes,
        string FollowUpInstructions,
        List<PrescriptionMedicalInfoDto> Prescriptions,
        AppointmentInfoDto Appointment,
        FacilityInfoDto Facility
    );

    public record AppointmentInfoDto(
        Guid Id,
        DateOnly ScheduledDate,
        TimeSpan ScheduledTime,
        string Status,
        string Notes
    );

    public record PrescriptionMedicalInfoDto(
        Guid Id,
        DateTime DateIssued,
        string MedicationList,
        string DosageInstructions
    );

    public record FacilityInfoDto(
        Guid Id,
        string Name,
        string Address
    );

    // Updated DTOs for allergies and chronic diseases based on your domain
    public record AllergyDto(
        Guid Id,
        string Name, // This will be the enum value as string
        AllergyType AllergyType // Optional: include the enum value if needed
    );

    public record ChronicDiseaseDto(
        Guid Id,
        string Name, // This will be the enum value as string
        ChronicDiseaseType ChronicDiseaseType // Optional: include the enum value if needed
    );
}