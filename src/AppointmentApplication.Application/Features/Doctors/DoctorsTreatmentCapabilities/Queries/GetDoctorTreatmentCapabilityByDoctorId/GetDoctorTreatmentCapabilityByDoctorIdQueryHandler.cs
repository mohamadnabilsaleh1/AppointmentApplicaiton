using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Dtos;
using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Errors;
using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Mappers;
using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Queries.GetDoctorTreatmentCapabilityByDoctorId;
using AppointmentApplication.Application.Features.Doctors.Errors;
using AppointmentApplication.Application.Features.Doctors.Schedules.Mapper;
using AppointmentApplication.Application.HealthcareFacilities.Schedules.Queries;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Queries.GetDoctorTreatmentCapabilityDoctorId
{
    public class GetDoctorTreatmentCapabilityByDoctorIdQueryHandler
        : IRequestHandler<GetDoctorTreatmentCapabilityByDoctorIdQuery, Result<DoctorTreatmentCapabilityDto>>
    {
        private readonly IAppDbContext _context;

        public GetDoctorTreatmentCapabilityByDoctorIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<DoctorTreatmentCapabilityDto>> Handle(
            GetDoctorTreatmentCapabilityByDoctorIdQuery request,
            CancellationToken cancellationToken)
        {
            var doctor = await _context.Doctors
                .Include(d => d.TreatmentCapacity)
                .FirstOrDefaultAsync(d => d.Id == request.DoctorId, cancellationToken);

            if (doctor is null)
            {
                return ApplicationDoctorErrors.DoctorNotFound(request.DoctorId);
            }

            if (doctor.TreatmentCapacity is null)
            {
                return ApplicationDoctorTreatmentCapabilityErrors.DoctorTreatmentCapabilityNotFound(request.DoctorId);
            }

            return doctor.TreatmentCapacity.ToDto();
        }
    }
}
