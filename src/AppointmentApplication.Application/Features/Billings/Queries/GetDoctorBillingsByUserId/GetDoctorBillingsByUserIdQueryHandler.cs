using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Billings.Dtos;
using AppointmentApplication.Application.Features.Billings.Erros;
using AppointmentApplication.Application.Features.Billings.Mappers;
using AppointmentApplication.Application.Features.Billings.Queries.GetDoctorBillingsByUserId;
using AppointmentApplication.Application.Features.Doctors.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Query;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Billings;
using AppointmentApplication.Domain.Billings.Enums;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Billings.Queries.GetDoctorBillingsByUserId
{
    public class GetDoctorBillingsByUserIdQueryHandler : IRequestHandler<GetDoctorBillingsByUserIdQuery, Result<PaginationResult<ExpandoObject>>>
    {
        private readonly IAppDbContext _context;
        private readonly DynamicQueryService _dynamicQueryService;
        private readonly DataShapingService _dataShapingService;
        private readonly ILogger<GetDoctorBillingsByUserIdQueryHandler> _logger;

        public GetDoctorBillingsByUserIdQueryHandler(
            IAppDbContext context,
            DataShapingService dataShapingService,
            DynamicQueryService dynamicQueryService,
            ILogger<GetDoctorBillingsByUserIdQueryHandler> logger)
        {
            _context = context;
            _dataShapingService = dataShapingService;
            _dynamicQueryService = dynamicQueryService;
            _logger = logger;
        }

        public async Task<Result<PaginationResult<ExpandoObject>>> Handle(
            GetDoctorBillingsByUserIdQuery request,
            CancellationToken cancellationToken)
        {

            _logger.LogInformation("Getting billings for doctor user: {UserId}", request.UserId);

            // 1. Find the doctor by UserId
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == request.UserId && d.IsActive, cancellationToken);

            if (doctor is null)
            {
                _logger.LogWarning("Doctor not found for user: {UserId}", request.UserId);
                return ApplicationDoctorErrors.DoctorNotFound(request.UserId);
            }

            _logger.LogInformation("Found doctor: {DoctorId} for user: {UserId}", doctor.Id, request.UserId);

            // 2. Start with base query for billings
            IQueryable<Billing> query = _context.Billings
                .Include(b => b.Doctor)
                .Include(b => b.Patient)
                .Include(b => b.Appointment)
                .Where(b => b.DoctorId == doctor.Id);

            // 3. Apply date filters
            if (request.StartDate.HasValue)
            {
                query = query.Where(b => b.DateIssued >= request.StartDate.Value);
                _logger.LogInformation("Applied start date filter: {StartDate}", request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(b => b.DateIssued <= request.EndDate.Value);
                _logger.LogInformation("Applied end date filter: {EndDate}", request.EndDate.Value);
            }

            // 4. Apply status filter
            if (!string.IsNullOrEmpty(request.Status))
            {
                if (Enum.TryParse<BillingStatus>(request.Status, true, out var status))
                {
                    query = query.Where(b => b.Status == status);
                    _logger.LogInformation("Applied status filter: {Status}", status);
                }
                else
                {
                    _logger.LogWarning("Invalid status filter: {Status}", request.Status);
                    return ApplicationBillingErrors.InvalidStatusFilter(request.Status);
                }
            }

            // 5. Prepare filters dictionary for dynamic service
            var filters = new Dictionary<string, object?>();

            // 6. Execute dynamic query service
            var dynamicQueryResult = await _dynamicQueryService.ExecuteAsync<Billing, BillingDto>(
                query: query,
                searchTerm: request.Search,
                searchProperties: new[]
                {
                        "Patient.FirstName",
                        "Patient.LastName",
                        "Doctor.FirstName",
                        "Doctor.LastName"
                },
                sortBy: request.Sort ?? "DateIssued desc",
                page: request.Page,
                pageSize: request.PageSize,
                fields: request.Fields,
                toDtoFunc: list =>
                {
                    _logger.LogInformation("Mapping {Count} billings to DTOs", list.Count);
                    return list.ToDtos();
                },
                filters: filters);

            _logger.LogInformation("Successfully retrieved billings for doctor {DoctorId}", doctor.Id);
            return dynamicQueryResult;
        }
    }
}