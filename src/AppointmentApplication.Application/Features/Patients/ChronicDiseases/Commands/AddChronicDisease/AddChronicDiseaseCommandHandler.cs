

using AppointmentApplication.Application.Features.Patients.Commands.AddAllergy;
using AppointmentApplication.Application.Features.Patients.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Patients.ChronicDiseases;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Patients.Commands.AddChronicDisease;

public class AddChronicDiseaseCommandHandler : IRequestHandler<AddChronicDiseaseCommand, Result<Created>>
{
    private readonly IAppDbContext _context;

    // ✅ Constructor Injection
    public AddChronicDiseaseCommandHandler(IAppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Result<Created>> Handle(AddChronicDiseaseCommand request, CancellationToken cancellationToken)
    {
        var chronicDisease = await _context.ChronicDiseases
            .FirstOrDefaultAsync(a => a.Name == request.ChronicDiseaseType, cancellationToken) ?? ChronicDisease.GetAll().FirstOrDefault(a => a.Name == request.ChronicDiseaseType);
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (patient is null)
        {
            return ApplicationPatientErrors.PatientNotFound(request.UserId);
        }

        patient.AddChronicDiseases(chronicDisease!);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Created;
    }
}
