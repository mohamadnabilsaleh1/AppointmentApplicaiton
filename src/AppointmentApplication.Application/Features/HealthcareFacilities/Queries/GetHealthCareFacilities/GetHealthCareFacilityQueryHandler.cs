using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Query;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilities
{
    public class GetHealthCareFacilityQueryHandler(
        IAppDbContext context,
        DataShapingService dataShapingService,
        DynamicQueryService dynamicQueryService
    ) : IRequestHandler<GetHealthCareFacilityQuery, Result<PaginationResult<ExpandoObject>>>
    {
        private readonly IAppDbContext _context = context;
        private readonly DynamicQueryService _dynamicQueryService = dynamicQueryService;

        public async Task<Result<PaginationResult<ExpandoObject>>> Handle(
            GetHealthCareFacilityQuery request,
            CancellationToken cancellationToken)
        {
            var query = _context.HealthcareFacilities
                .Include(f => f.Departments)
                .Include(f => f.Schedules)
                .Include(f => f.ScheduleExceptions)
                .AsQueryable();

            Console.WriteLine("hello");
            Console.WriteLine(request.City);

            // ✅ Build filters dictionary
            var filters = new Dictionary<string, object?>
            {
                { "Type", request.Type },
                { "Address.Street", request.Street },
                { "Address.City", request.City },
                { "Address.State", request.State },
                { "Address.Country", request.Country },
                { "Address.ZipCode", request.ZipCode }
            };

            var result = await _dynamicQueryService.ExecuteAsync<HealthCareFacility, HealthcareFacilityDto>(
                query: query,
                searchTerm: request.Search,
                searchProperties: new[] { "Name" },
                sortBy: request.Sort,
                page: request.Page,
                pageSize: request.PageSize,
                fields: request.Fields,
                toDtoFunc: list => list.ToDtos(),
                filters: filters);

            return result;
        }
    }
}
