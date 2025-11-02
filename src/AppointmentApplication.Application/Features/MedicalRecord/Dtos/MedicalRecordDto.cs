// AppointmentApplication.Application/Features/MedicalRecords/Dtos/MedicalRecordDtos.cs
using System;
using System.Collections.Generic;

using AppointmentApplication.Application.Features.MedicalRecords.Dtos;

namespace AppointmentApplication.Application.Features.MedicalRecords.Dtos
{
    // Main DTO as requested with medicalRecord as array
    public record MedicalRecordDto(
        Guid Id,
        string PatientFullName,
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
}

// AppointmentApplication.Application/Features/MedicalRecords/Dtos/MedicalRecordDtos.cs (إضافة)
public record MedicalRecordForDoctorDto(
    Guid Id,
    string PatientFullName,
    string PatientNationalId,
    string PatientGender,
    int PatientAge,
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

