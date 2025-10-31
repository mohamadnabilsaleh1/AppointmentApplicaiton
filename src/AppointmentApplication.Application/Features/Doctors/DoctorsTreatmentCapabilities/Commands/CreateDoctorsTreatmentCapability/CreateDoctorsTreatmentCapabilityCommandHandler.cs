using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Dtos;
using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Errors;
using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Mappers;
using AppointmentApplication.Application.Features.Doctors.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Doctors.DoctorsTreatmentCapabilities;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Commands.CreateDoctorsTreatmentCapability
{
    public class CreateDoctorsTreatmentCapabilityCommandHandler
        : IRequestHandler<CreateDoctorsTreatmentCapabilityCommand, Result<DoctorTreatmentCapabilityDto>>
    {
        private readonly IAppDbContext _context;

        public CreateDoctorsTreatmentCapabilityCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<DoctorTreatmentCapabilityDto>> Handle(
            CreateDoctorsTreatmentCapabilityCommand request,
            CancellationToken cancellationToken)
        {
            // Find the doctor
            var doctor = await _context.Doctors
            .Include(d => d.TreatmentCapacity)
            .FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);

            if (doctor is null)
            {
                return ApplicationDoctorErrors.DoctorNotFound(request.UserId);
            }
            if (doctor.TreatmentCapacity is not null)
            {
                return ApplicationDoctorTreatmentCapabilityErrors.DoctorTreatmentCapabilityAlreadyExist(doctor.Id);
            }

            // Create treatment capability
            var treatmentCapabilityResult = doctor.CreateTreatmentCapacity(
                doctor.Id,
                request.MaxPatientsPerDay,
                request.SessionDurationMinutes);

            if (treatmentCapabilityResult.IsError)
            {
                return treatmentCapabilityResult.Errors;
            }

            var treatmentCapability = treatmentCapabilityResult.Value;
            _context.DoctorTreatmentCapacities.Add(treatmentCapability);

            // Save to database
            await _context.SaveChangesAsync(cancellationToken);

            // Return DTO
            return treatmentCapability.ToDto();

        }
    }
}