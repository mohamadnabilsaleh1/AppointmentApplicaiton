// AppointmentApplication.Application/Features/MedicalRecords/Queries/GetMedicalRecordsForDoctorByPatientId/GetMedicalRecordsForDoctorByPatientIdQuery.cs
using AppointmentApplication.Application.Features.MedicalRecords.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.MedicalRecords.Queries.GetMedicalRecordsForDoctorByPatientId
{
    public sealed record GetMedicalRecordsForDoctorByPatientIdQuery(
        Guid UserId,
        Guid PatientId
    ) : IRequest<Result<MedicalRecordForDoctorDto>>;
}