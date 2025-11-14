using System;
using AppointmentApplication.Application.Features.Reviews.Commands.CreateReview;
using AppointmentApplication.Application.Features.Reviews.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Reviews.Queries.GetReviewByAppointmentId
{
    public sealed record GetReviewByAppointmentIdQuery(Guid AppointmentId) : IRequest<Result<ReviewDto>>;
}