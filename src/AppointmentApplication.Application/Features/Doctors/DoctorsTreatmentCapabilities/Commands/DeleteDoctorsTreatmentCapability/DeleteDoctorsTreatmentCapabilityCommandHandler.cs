// AppointmentApplication.Application/Features/Doctors/DoctorsTreatmentCapabilities/Commands/DeleteDoctorsTreatmentCapability/DeleteDoctorsTreatmentCapabilityCommandHandler.cs
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Commands.CreateDoctorsTreatmentCapability;
using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Errors;
using AppointmentApplication.Application.Features.Doctors.Errors;
using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Commands.DeleteDoctorsTreatmentCapability
{
    public class DeleteDoctorsTreatmentCapabilityCommandHandler
        : IRequestHandler<DeleteDoctorsTreatmentCapabilityCommand, Result<Deleted>>
    {
        private readonly IAppDbContext _context;

        public DeleteDoctorsTreatmentCapabilityCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        // AppointmentApplication.Application/Features/Doctors/DoctorsTreatmentCapabilities/Commands/DeleteDoctorsTreatmentCapability/DeleteDoctorsTreatmentCapabilityCommandHandler.cs
        public async Task<Result<Deleted>> Handle(
            DeleteDoctorsTreatmentCapabilityCommand request,
            CancellationToken cancellationToken)
        {
            // Find the doctor with treatment capacity
            var doctor = await _context.Doctors
                .Include(d => d.TreatmentCapacity)
                .FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);

            if (doctor is null)
            {
                return ApplicationDoctorErrors.DoctorNotFound(request.UserId);
            }

            if (doctor.TreatmentCapacity is null)
            {
                return ApplicationDoctorTreatmentCapabilityErrors.DoctorTreatmentCapabilityNotFound(doctor.Id);
            }

            // ✅ Store reference BEFORE calling domain method
            var treatmentCapacityToRemove = doctor.TreatmentCapacity;

            // Call domain method for validation
            var deleteResult = doctor.DeleteTreatmentCapacity();
            if (deleteResult.IsError)
            {
                return deleteResult.Errors;
            }

            // ✅ Remove using the stored reference
            _context.DoctorTreatmentCapacities.Remove(treatmentCapacityToRemove);

            // Save changes
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }
    }
}