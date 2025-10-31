using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Dtos;
using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Errors;
using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Mappers;
using AppointmentApplication.Application.Features.Doctors.Errors;
using AppointmentApplication.Application.Features.Doctors.Schedules.Mapper;
using AppointmentApplication.Application.HealthcareFacilities.Schedules.Queries;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Queries.GetDoctorTreatmentCapabilityById
{
    public class GetDoctorTreatmentCapabilityByIdQueryHandler
        : IRequestHandler<GetDoctorTreatmentCapabilityByIdQuery, Result<DoctorTreatmentCapabilityDto>>
    {
        private readonly IAppDbContext _context;

        public GetDoctorTreatmentCapabilityByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<DoctorTreatmentCapabilityDto>> Handle(
            GetDoctorTreatmentCapabilityByIdQuery request,
            CancellationToken cancellationToken)
        {
            var doctor = await _context.Doctors
                .Include(d => d.TreatmentCapacity)
                .FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);

            if (doctor is null)
            {
                return ApplicationDoctorErrors.DoctorNotFound(request.UserId);
            }

            if (doctor.TreatmentCapacity is null)
            {
                return ApplicationDoctorTreatmentCapabilityErrors.DoctorTreatmentCapabilityNotFound(request.UserId);
            }

            return doctor.TreatmentCapacity.ToDto();
        }
    }
}
