using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Contracts.Requests.Reviews
{
    public sealed record CreateReviewRequest(
        // Guid AppointmentId,
        int Rating,
        string? Comment = null
    );
}