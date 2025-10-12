using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Dtos;
using AppointmentApplication.Application.HealthcareFacilities.Patients.Uploads.Mappers;

using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Uploads.Commands.ChangeFileToPrivate
{
    public class GetUploadedFilesByUserIdQueryHandler : IRequestHandler<GetUploadedFilesByUserIdQuery, Result<List<UploadDto>>>
    {
        private readonly IAppDbContext _context;
        public GetUploadedFilesByUserIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<UploadDto>>> Handle(GetUploadedFilesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var healthCareFacility = await _context.HealthcareFacilities
            .Include(p => p.Uploads)
            .FirstOrDefaultAsync(p => p.UserId == request.UserId);
            if (healthCareFacility is null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
            }
            var uploads = healthCareFacility.Uploads.ToDtos();
            return uploads;
        }

    }
}