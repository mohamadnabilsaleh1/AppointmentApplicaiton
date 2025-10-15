using AppointmentApplication.Application.Features.Patients.Commands.AddAllergy;
using AppointmentApplication.Application.Features.Patients.Commands.AddChronicDisease;
using AppointmentApplication.Application.Features.Patients.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Patients.ChronicDiseases;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Patients.Commands.AddChronicDisease;

public class AddChronicDiseaseCommandHandler : IRequestHandler<AddChronicDiseaseCommand, Result<ChronicDisease>>
{
    private readonly IAppDbContext _context;

    // ✅ Constructor Injection
    public AddChronicDiseaseCommandHandler(IAppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Result<ChronicDisease>> Handle(AddChronicDiseaseCommand request, CancellationToken cancellationToken)
    {
        // جلب المريض مع الأمراض المزمنة المرتبطة
        var patient = await _context.Patients
            .Include(p => p.ChronicDiseases)
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (patient is null)
        {
            return ApplicationPatientErrors.PatientNotFound(request.UserId);
        }

        // ✅ جلب مرض مزمن موجود مسبقًا من قاعدة البيانات
        var chronicDisease = await _context.ChronicDiseases
            .FirstOrDefaultAsync(cd => cd.Name == request.ChronicDiseaseType, cancellationToken);

        if (chronicDisease is null)
        {
            return ApplicationPatientErrors.InvalidChronicDiseaseType;
        }

        // ✅ إضافة المرض المزمن للمريض إذا لم يكن موجودًا مسبقًا
        var result = patient.AddChronicDisease(chronicDisease);

        if (result.IsError)
        {
            return result.Errors;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return chronicDisease;
    }
}
