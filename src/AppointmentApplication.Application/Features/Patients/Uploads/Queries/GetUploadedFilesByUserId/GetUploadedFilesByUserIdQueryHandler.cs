// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// using AppointmentApplication.Application.Features.Patients.Errors;

// using AppointmentApplication.Application.Features.Patients.Uploads.Dtos;
// using AppointmentApplication.Application.Features.Patients.Uploads.Mappers;

// using AppointmentApplication.Application.Features.Patients.Uploads.Queries.GetUploadedFileByUserIdQuery;
// using AppointmentApplication.Application.Shared.Interfaces;
// using AppointmentApplication.Domain.Shared.Results;

// using MediatR;

// using Microsoft.EntityFrameworkCore;

// namespace AppointmentApplication.Application.Features.Patients.Uploads.Queries.GetUploadedFilesByUserId
// {
//     public class GetUploadedFilesByUserIdQueryHandler : IRequestHandler<GetUploadedFilesByUserIdQuery, Result<List<UploadDto>>>
//     {
//         private readonly IAppDbContext _context;
//         public GetUploadedFilesByUserIdQueryHandler(IAppDbContext context)
//         {
//             _context = context;
//         }
//         public async Task<Result<List<UploadDto>>> Handle(GetUploadedFilesByUserIdQuery request, CancellationToken cancellationToken)
//         {
//             var patient = await _context.Patients
//             .Include(p => p.Uploads)
//             .FirstOrDefaultAsync(p => p.UserId == request.UserId);
//             if (patient is null)
//             {
//                 return ApplicationPatientErrors.PatientNotFound(request.UserId);
//             }
//             var uploads = patient.Uploads.ToDtos();
//             return uploads;
//         }

//     }
// }