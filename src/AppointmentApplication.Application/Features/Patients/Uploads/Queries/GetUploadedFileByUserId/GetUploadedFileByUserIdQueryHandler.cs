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

// namespace AppointmentApplication.Application.Features.Patients.Uploads.Queries.GetUploadedFileByUserId
// {
//     public class GetUploadedFileByUserIdQueryHandler : IRequestHandler<GetUploadedFileByUserIdQuery, Result<UploadDto>>
//     {
//         private readonly IAppDbContext _context;
//         public GetUploadedFileByUserIdQueryHandler(IAppDbContext context)
//         {
//             _context = context;
//         }
//         public async Task<Result<UploadDto>> Handle(GetUploadedFileByUserIdQuery request, CancellationToken cancellationToken)
//         {
//             var patient = await _context.Patients.Include(p => p.Uploads).FirstOrDefaultAsync(p => p.UserId == request.UserId);
//             if (patient == null)
//             {
//                 return ApplicationPatientErrors.PatientNotFound(request.UserId);
//             }
//             var uploadResult = patient.GetUploadedById(request.UploadedId);
//             if (uploadResult.IsError)
//             {
//                 return uploadResult.Errors;
//             }
//             return uploadResult.Value.ToDto();
//         }
//     }
// }