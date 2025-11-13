using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Reviews.Dtos;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Reviews.Commands.CreateReview
{
public sealed record CreateReviewCommand(
    Guid UserId,
    Guid AppointmentId,
    int Rating,
    string? Comment = null
) : IRequest<Result<ReviewDto>>;
}