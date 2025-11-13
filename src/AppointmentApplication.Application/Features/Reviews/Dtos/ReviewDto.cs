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

}