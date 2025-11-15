using AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Users.GetAvatar
{
    public sealed record GetAvatarQuery(Guid UserId) : IRequest<Result<FileUploadResponse>>;
}