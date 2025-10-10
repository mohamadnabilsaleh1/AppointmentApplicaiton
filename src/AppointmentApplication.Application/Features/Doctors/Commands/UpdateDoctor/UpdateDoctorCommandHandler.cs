
using AppointmentApplication.Application.Features.Doctors.Commands.UpdateDoctor;
using AppointmentApplication.Application.Features.Doctors.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;

public class UpdateDcotorCommandHandler
    : IRequestHandler<UpdateDoctorCommand, Result<Updated>>
{
    private readonly ILogger<UpdateDcotorCommandHandler> _logger;
    private readonly IAppDbContext _context;

    public UpdateDcotorCommandHandler(
        ILogger<UpdateDcotorCommandHandler> logger,
        IAppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Updated>> Handle(
        UpdateDoctorCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Find the existing doctor
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);

        if (doctor is null)
        {
            _logger.LogWarning("Doctor  not found. ID: {DoctorId}", request.UserId);
            return ApplicationDoctorErrors.DoctorNotFound(request.UserId);
        }

        // 6. Update the doctor
        var updateResult = doctor.Update(request.FirstName,request.LastName,request.Gender,request.DateOfBirth);

        if (updateResult.IsError)
        {
            _logger.LogWarning(
                "Doctor facility update failed: {Errors}",
                string.Join(", ", updateResult.Errors));
            return updateResult.Errors;
        }

        // 7. Save changes
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}