// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// using AppointmentApplication.Application.Features.Patients.Errors;

// using AppointmentApplication.Application.Features.Patients.Uploads.Dtos;
// using AppointmentApplication.Application.Features.Patients.Uploads.Mappers;
// using AppointmentApplication.Application.Shared.Interfaces;
// using AppointmentApplication.Domain.Shared.Results;

// using MediatR;

// using Microsoft.EntityFrameworkCore;

// namespace AppointmentApplication.Application.Features.Patients.Uploads.Queries.GetUploadedFileByPatientId
// {
//     public class GetUploadedFilesQueryHandler : IRequestHandler<GetUploadedFilesQuery, Result<List<UploadDto>>>
//     {
//         private readonly IAppDbContext _context;
//         public GetUploadedFilesQueryHandler(IAppDbContext context)
//         {
//             _context = context;
//         }
//         public async Task<Result<List<UploadDto>>> Handle(GetUploadedFilesQuery request, CancellationToken cancellationToken)
//         {
//             var patient = await _context.Patients
//                 .Include(p => p.Uploads)
//                 .FirstOrDefaultAsync(p => p.Id == request.PatientId);
//             if (patient == null)
//             {
//                 return ApplicationPatientErrors.PatientNotFound(request.PatientId);
//             }
//             var uploads = patient.Uploads.ToDtos();
//             return uploads;
//         }

//     }

// }