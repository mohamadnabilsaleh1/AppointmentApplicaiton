using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;
using AppointmentApplication.Application.Features.Doctors.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Query;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.Doctors.Enums;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.Queries.GetDoctors
{
    public class GetDoctorsQueryHandler : IRequestHandler<GetDoctorsQuery, Result<PaginationResult<ExpandoObject>>>
    {
        private readonly IAppDbContext _context;
        private readonly DynamicQueryService _dynamicQueryService;
        private readonly DataShapingService _dataShapingService;

        public GetDoctorsQueryHandler(
            IAppDbContext context,
            DataShapingService dataShapingService,
            DynamicQueryService dynamicQueryService)
        {
            _context = context;
            _dataShapingService = dataShapingService;
            _dynamicQueryService = dynamicQueryService;
        }

        public async Task<Result<PaginationResult<ExpandoObject>>> Handle(
            GetDoctorsQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<Doctor> query = _context.Doctors
                .Include(d => d.User) // Include User data
                    .ThenInclude(u => u.Emails) // Include Emails
                .Include(d => d.User)
                    .ThenInclude(u => u.Phones) // Include Phones
                .AsQueryable();

            // Apply filters
            var filters = new Dictionary<string, object?>
    {
        { "Specialization", request.Specialization }
    };

            // Remove null filters
            filters = filters.Where(f => f.Value != null)
                           .ToDictionary(f => f.Key, f => f.Value);

            // Execute dynamic query service to get filtered IQueryable
            var dynamicQueryResult = await _dynamicQueryService.ExecuteAsync<Doctor, DoctorWithContactDto>(
                query: query,
                searchTerm: request.Search,
                searchProperties: new[] { "FirstName", "LastName" },
                sortBy: request.Sort,
                page: request.Page,
                pageSize: request.PageSize,
                fields: request.Fields,
                toDtoFunc: list => list.ToDtosWithContact(), // Use new mapping method
                filters: filters);

            return dynamicQueryResult;
        }
    }
}