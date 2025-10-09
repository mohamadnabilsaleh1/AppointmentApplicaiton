// 📁 Application/HealthcareFacilities/ScheduleExceptions/Commands/UpdateScheduleExceptionCommandHandler.cs
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Errors;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Mappers;
using AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Commands;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.ScheduleExceptions.Mappers;
public class UpdateScheduleExceptionCommandHandler : IRequestHandler<UpdateScheduleExceptionCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;

    public UpdateScheduleExceptionCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Updated>> Handle(UpdateScheduleExceptionCommand request, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .Include(f => f.ScheduleExceptions)
            .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

        if (doctor is null)
        {
            return ApplicationDoctorErrors.DoctorNotFound(request.UserId);
        }

        var updateResult = doctor.UpdateScheduleException(
            request.ExceptionId,
            request.Date,
            request.StartTime,
            request.EndTime,
            request.Status,
            request.Reason);

        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}