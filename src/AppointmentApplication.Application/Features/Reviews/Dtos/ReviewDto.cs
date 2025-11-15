using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Application.Features.Reviews.Dtos
{
    public sealed record ReviewDto(
        Guid Id,
        Guid PatientId,
        Guid DoctorId,
        Guid FacilityId,
        Guid AppointmentId,
        int Rating,
        string Comment,
        DateTime CreatedAt,
        string PatientName,
        string DoctorName,
        string FacilityName
    );
        public sealed record HealthCareFacilityReviewDto(
        Guid Id,
        int Rating,
        string Comment,
        DateTime CreatedAtUtc,
        DateTime? UpdatedAtUtc,
        Guid AppointmentId,
        Guid PatientId,
        string PatientFirstName,
        string PatientLastName,
        string PatientFullName,
        string PatientEmail,
        Guid DoctorId,
        string DoctorFirstName,
        string DoctorLastName,
        string DoctorSpecialization, // This is now string type
        string DoctorFullName,
        Guid FacilityId,
        string FacilityName,
        DateOnly? AppointmentDate,
        TimeSpan? AppointmentTime,
        string AppointmentStatus
    );

}