// 📁 Application/HealthcareFacilities/ScheduleExceptions/Commands/CreateScheduleExceptionCommandHandler.cs
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Mappers;
using AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Commands;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.ScheduleExceptions.Mappers;
public class CreateScheduleExceptionCommandHandler : IRequestHandler<CreateScheduleExceptionCommand, Result<ScheduleExceptionDto>>
{
    private readonly IAppDbContext _context;

    public CreateScheduleExceptionCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ScheduleExceptionDto>> Handle(CreateScheduleExceptionCommand request, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .Include(f => f.ScheduleExceptions)
            .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

        if (doctor is null)
        {
            return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
        }

        var exceptionResult = doctor.AddScheduleException(
            request.Date,
            request.StartTime,
            request.EndTime,
            request.Status,
            request.Reason);

        if (exceptionResult.IsError)
        {
            return exceptionResult.Errors;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return exceptionResult.Value.ToDto();
    }
}