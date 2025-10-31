using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Errors;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Mappers;
using AppointmentApplication.Application.HealthcareFacilities.Schedules.Commands;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Doctors.DoctorsTreatmentCapabilities;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Commands.UpdateDoctorsTreatmentCapability
{
    public class UpdateDoctorsTreatmentCapabilityCommandHandler : IRequestHandler<UpdateDoctorsTreatmentCapabilityCommand, Result<Updated>>
    {
        private readonly IAppDbContext _context;

        public UpdateDoctorsTreatmentCapabilityCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Updated>> Handle(UpdateDoctorsTreatmentCapabilityCommand request, CancellationToken cancellationToken)
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
                return ApplicationDoctorErrors.DoctorTreatmentCapacityNotFound(request.UserId);
            }

            var updateResult = doctor.UpdateTreatmentCapacity(
                request.MaxPatientsPerDay,
                request.SessionDurationMinutes);

            if (updateResult.IsError)
            {
                return updateResult.Errors;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Updated;
        }
    }
}



