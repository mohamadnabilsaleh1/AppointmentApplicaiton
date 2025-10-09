// 📁 Application/HealthcareFacilities/ScheduleExceptions/Commands/DeleteScheduleExceptionCommandHandler.cs
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Errors;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Commands;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.ScheduleExceptions.Mappers;
public class DeleteScheduleExceptionCommandHandler : IRequestHandler<DeleteScheduleExceptionCommand, Result<Deleted>>
{
    private readonly IAppDbContext _context;

    public DeleteScheduleExceptionCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Deleted>> Handle(DeleteScheduleExceptionCommand request, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .Include(f => f.ScheduleExceptions)
            .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

        if (doctor is null)
        {
            return ApplicationDoctorErrors.DoctorNotFound(request.UserId);
        }

        var deleteResult = doctor.DeleteScheduleException(request.ExceptionId);
        if (deleteResult.IsError)
        {
            return deleteResult.Errors;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}