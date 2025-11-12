// AppointmentApplication.Application/Features/MedicalRecords/Queries/GetAllMedicalRecords/GetAllMedicalRecordsQuery.cs
using AppointmentApplication.Application.Features.MedicalRecords.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.MedicalRecords.Queries.GetAllMedicalRecords
{
    public sealed record GetMedicalRecordForPaitnetByUserIdQuery(Guid UserId)
        : IRequest<Result<List<MedicalRecordDto>>>;
}