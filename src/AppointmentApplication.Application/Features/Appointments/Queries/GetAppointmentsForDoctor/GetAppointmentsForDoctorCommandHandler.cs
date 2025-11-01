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
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Query;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Appointments.Enums;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Appointments.Queries.GetAppointmentsForDoctor
{
    public class GetAppointmentsForDoctorCommandHandler : IRequestHandler<GetAppointmentsForDoctorCommand, Result<PaginationResult<ExpandoObject>>>
    {
        private readonly IAppDbContext _context;
        private readonly DynamicQueryService _dynamicQueryService;
        private readonly DataShapingService _dataShapingService;
        private readonly ILogger<GetAppointmentsForDoctorCommandHandler> _logger;

        public GetAppointmentsForDoctorCommandHandler(
            IAppDbContext context,
            DataShapingService dataShapingService,
            DynamicQueryService dynamicQueryService,
            ILogger<GetAppointmentsForDoctorCommandHandler> logger)
        {
            _context = context;
            _dataShapingService = dataShapingService;
            _dynamicQueryService = dynamicQueryService;
            _logger = logger;
        }

        public async Task<Result<PaginationResult<ExpandoObject>>> Handle(
            GetAppointmentsForDoctorCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting appointments for doctor with filters - Status: {Status}", request.Status);

            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == request.UserId);
            if (doctor is null)
            {
                return ApplicationDoctorErrors.DoctorNotFound(request.UserId);
            }

            // Start with base query with all required includes
            IQueryable<Appointment> query = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Facility)
                .Where(a => a.DoctorId == doctor.Id);

            // ✅ **FIX: Apply status filter directly to the query**
            if (request.Status.HasValue)
            {
                query = query.Where(a => a.Status == request.Status.Value);
                _logger.LogInformation("Applied status filter: {Status}", request.Status.Value);
            }

            // Apply date filters
            if (request.StartDate.HasValue)
            {
                query = query.Where(a => a.ScheduledDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(a => a.ScheduledDate <= request.EndDate.Value);
            }

            // Prepare filters dictionary (for dynamic query service if needed)
            var filters = new Dictionary<string, object?>();
            
            // Note: We already applied status filter above, but we can keep it for dynamic service if needed
            if (request.Status.HasValue)
            {
                filters.Add("Status", request.Status.Value);
            }

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
                toDtoFunc: list => 
                {
                    _logger.LogInformation("Mapped {Count} appointments with status filter", list.Count);
                    return list.ToDtos();
                },
                filters: filters); // Pass filters to dynamic service

            return dynamicQueryResult;
        }
    }
}