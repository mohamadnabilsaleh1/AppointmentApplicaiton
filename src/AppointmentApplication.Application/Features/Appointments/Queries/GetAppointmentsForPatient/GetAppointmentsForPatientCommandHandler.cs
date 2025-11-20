using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Appointments.Dtos;
using AppointmentApplication.Application.Features.Appointments.Mappers;
using AppointmentApplication.Application.Features.Appointments.Queries.GetAppointmentByDoctorId;
using AppointmentApplication.Application.Features.Doctors.Errors;
using AppointmentApplication.Application.Features.Patients.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Query;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Appointments.Enums;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Appointments.Queries.GetAppointmentsForDoctor
{
    public class GetAppointmentsForPatientCommandHandler : IRequestHandler<GetAppointmentsForPatientCommand, Result<PaginationResult<ExpandoObject>>>
    {
        private readonly IAppDbContext _context;
        private readonly DynamicQueryService _dynamicQueryService;
        private readonly DataShapingService _dataShapingService;

        public GetAppointmentsForPatientCommandHandler(
            IAppDbContext context,
            DataShapingService dataShapingService,
            DynamicQueryService dynamicQueryService)
        {
            _context = context;
            _dataShapingService = dataShapingService;
            _dynamicQueryService = dynamicQueryService;
        }

        public async Task<Result<PaginationResult<ExpandoObject>>> Handle(
            GetAppointmentsForPatientCommand request,
            CancellationToken cancellationToken)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(d => d.UserId == request.UserId);
            if(patient is null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }

            // Start with base query with all required includes
            IQueryable<Appointment> query = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Facility)

                // .Include(a => a.Billing) // Uncomment when Billing is ready
                // .Include(a => a.Prescriptions) // Uncomment when Prescriptions are ready
                .Where(a => a.PatientId == patient.Id);

            // Apply date filters
            if (request.StartDate.HasValue)
            {
                query = query.Where(a => a.ScheduledDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(a => a.ScheduledDate <= request.EndDate.Value);
            }

            // Prepare filters dictionary
            var filters = new Dictionary<string, object?>
            {
                { "Status", request.Status }
            };

            // Remove null filters
            filters = filters.Where(f => f.Value != null)
                           .ToDictionary(f => f.Key, f => f.Value);

            // Execute dynamic query service to get filtered and paginated results
            var dynamicQueryResult = await _dynamicQueryService.ExecuteAsync<Appointment, AppointmentDto>(
                query: query,
                searchTerm: request.Search,
                searchProperties: new[]
                {
                    "Patient.FirstName",
                    "Patient.LastName",
                    "Notes",
                    "Facility.Name",
                    "Doctor.FirstName",
                    "Doctor.LastName"
                },
                sortBy: request.Sort,
                page: request.Page,
                pageSize: request.PageSize,
                fields: request.Fields,
                toDtoFunc: list => list.ToDtos(),
                filters:filters);

            return dynamicQueryResult;
        }
    }
}