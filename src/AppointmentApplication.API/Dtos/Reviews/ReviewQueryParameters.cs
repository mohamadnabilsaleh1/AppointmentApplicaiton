using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.API.Dtos.Reviews
{
    public sealed record ReviewQueryParameters(
        string? Search = null,
        int Page = 1,
        int PageSize = 10,
        string? Sort = "CreatedAtUtc",
        string? Fields = null,
        int? MinRating = null,
        int? MaxRating = null,
        DateTime? FromDate = null,
        DateTime? ToDate = null,
        Guid? DoctorId = null,
        Guid? PatientId = null
    );
}