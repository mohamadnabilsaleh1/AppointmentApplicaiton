using System.Dynamic;

using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Mappers;
using AppointmentApplication.Application.Shared.Extensions;
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
            IQueryable<HealthCareFacility> query = _context.HealthcareFacilities
                .Include(f => f.Departments)
                .Include(f => f.Schedules)
                .Include(f => f.ScheduleExceptions)
                .Where(f => f.IsActive) // ✅ filter active facilities
                .AsQueryable();

            // Apply other dynamic filters first
            var filters = new Dictionary<string, object?>
    {
        { "Type", request.Type },
        { "Address.Street", request.Street },
        { "Address.City", request.City },
        { "Address.State", request.State },
        { "Address.Country", request.Country },
        { "Address.ZipCode", request.ZipCode }
    };

            // Execute dynamic query service to get filtered IQueryable
            var dynamicQueryResult = await _dynamicQueryService.ExecuteAsync<HealthCareFacility, HealthcareFacilityDto>(
                query: query,
                searchTerm: request.Search,
                searchProperties: new[] { "Name" },
                sortBy: request.Sort,
                page: request.Page,
                pageSize: request.PageSize,
                fields: request.Fields,
                toDtoFunc: list => list.ToDtos(),
                filters: filters
            );

            // إذا تم تحديد GPS، نحسب المسافة بعد التحويل لـ IEnumerable
            if (request.GPSLatitude.HasValue && request.GPSLongitude.HasValue)
            {
                double userLat = request.GPSLatitude.Value;
                double userLng = request.GPSLongitude.Value;
                double radiusKm = request.RadiusKm;

                var filteredList = dynamicQueryResult.Items
                    .Select(dto => new
                    {
                        Facility = dto,
                        Distance = CalculateDistance(
                            userLat,
                            userLng,
                            (double)((ExpandoObject)dto).GetPropertyValue("GPSLatitude"),
                            (double)((ExpandoObject)dto).GetPropertyValue("GPSLongitude")
                        )
                    })
                    .Where(x => x.Distance <= radiusKm)
                    .OrderBy(x => x.Distance)
                    .Select(x => x.Facility)
                    .Cast<ExpandoObject>()
                    .ToList();

                dynamicQueryResult = new PaginationResult<ExpandoObject>
                {
                    Items = filteredList,
                    Page = dynamicQueryResult.Page,
                    PageSize = dynamicQueryResult.PageSize,
                    TotalCount = filteredList.Count // تحديث العدد بعد التصفية
                };
            }

            return dynamicQueryResult;
        }

        // Haversine formula to calculate distance in KM between two coordinates
        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth's radius in km
            var dLat = Deg2Rad(lat2 - lat1);
            var dLon = Deg2Rad(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double Deg2Rad(double deg) => deg * (Math.PI / 180.0);
    }
}


