// 📁 Application/HealthcareFacilities/ScheduleExceptions/Queries/GetScheduleExceptionByIdQueryHandler.cs
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Mappers;
using AppointmentApplication.Application.HealthcareFacilities.ScheduleExceptions.Queries;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.ScheduleExceptions.Mappers;

public class GetScheduleExceptionByIdQueryHandler : IRequestHandler<GetScheduleExceptionByIdQuery, Result<ScheduleExceptionDto>>
{
    private readonly IAppDbContext _context;

    public GetScheduleExceptionByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ScheduleExceptionDto>> Handle(GetScheduleExceptionByIdQuery request, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .Include(f => f.ScheduleExceptions)
            .FirstOrDefaultAsync(f => f.Id == request.DoctorId, cancellationToken);

        if (doctor is null)
        {
            return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.DoctorId);
        }

        var exceptionResult = doctor.GetScheduleExceptionById(request.ScheduleExceptionId);
        if (exceptionResult.IsError)
        {
            return exceptionResult.Errors;
        }

        return exceptionResult.Value.ToDto();
    }
}