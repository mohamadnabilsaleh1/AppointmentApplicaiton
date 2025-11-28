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

namespace AppointmentApplication.Application.Features.Doctors.Queries.GetTopDoctors
{
    public class GetTopDoctorsQueryHandler : IRequestHandler<GetTopDoctorsQuery, Result<PaginationResult<ExpandoObject>>>
    {
        private readonly IAppDbContext _context;
        private readonly DynamicQueryService _dynamicQueryService;
        private readonly DataShapingService _dataShapingService;

        public GetTopDoctorsQueryHandler(
            IAppDbContext context,
            DataShapingService dataShapingService,
            DynamicQueryService dynamicQueryService)
        {
            _context = context;
            _dataShapingService = dataShapingService;
            _dynamicQueryService = dynamicQueryService;
        }

        public async Task<Result<PaginationResult<ExpandoObject>>> Handle(
            GetTopDoctorsQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<DoctorWithAggregates> query = _context.Doctors
                .Include(d => d.User)
                    .ThenInclude(u => u.Emails)
                .Include(d => d.User)
                    .ThenInclude(u => u.Phones)
                .Include(d => d.Reviews)
                .Select(d => new DoctorWithAggregates
                {
                    Id = d.Id,
                    Doctor = d,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Specialization = d.Specialization,
                    Rating = d.Reviews.Any() ? Math.Round(d.Reviews.Average(r => r.Rating), 1) : 0,
                    AverageRating = d.Reviews.Any() ? Math.Round(d.Reviews.Average(r => r.Rating), 1) : 0,
                    TotalReviews = d.Reviews.Count
                });

            var filters = new Dictionary<string, object?>
            {
                { "Specialization", request.Specialization }
            }
            .Where(f => f.Value != null)
            .ToDictionary(f => f.Key, f => f.Value);

            var sort = string.IsNullOrWhiteSpace(request.Sort)
                ? "Rating desc,TotalReviews desc,LastName asc,FirstName asc"
                : request.Sort;

            var dynamicQueryResult = await _dynamicQueryService.ExecuteAsync<DoctorWithAggregates, DoctorWithContactDto>(
                query: query,
                searchTerm: request.Search,
                searchProperties: new[] { "FirstName", "LastName" },
                sortBy: sort,
                page: request.Page,
                pageSize: request.PageSize,
                fields: request.Fields,
                toDtoFunc: list => list.Select(x => x.Doctor).ToDtosWithContact(),
                filters: filters);

            return dynamicQueryResult;
        }

        private sealed class DoctorWithAggregates
        {
            public Guid Id { get; set; }
            public Doctor Doctor { get; set; } = null!;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public Specialization Specialization { get; set; }
            public double Rating { get; set; }
            public double AverageRating { get; set; }
            public int TotalReviews { get; set; }
        }
    }
}
