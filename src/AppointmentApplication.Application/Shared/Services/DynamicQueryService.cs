using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
using AppointmentApplication.Application.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Shared.Query
{
    /// <summary>
    /// خدمة ديناميكية شاملة للبحث، الترتيب، التصفية، الصفحات، وتشكيل البيانات
    /// </summary>
    public class DynamicQueryService
    {
        private readonly DataShapingService _dataShapingService;

        public DynamicQueryService(DataShapingService dataShapingService)
        {
            _dataShapingService = dataShapingService;
        }

        public async Task<PaginationResult<ExpandoObject>> ExecuteAsync<TEntity, TDto>(
            IQueryable<TEntity> query,
            string? searchTerm,
            string[]? searchProperties,
            string? sortBy,
            int page,
            int pageSize,
            string? fields,
            Func<List<TEntity>, List<TDto>> toDtoFunc,
            Dictionary<string, object?>? filters = null)
            where TEntity : class
        {
            // ---------------------
            // 1. البحث الديناميكي
            // ---------------------
            if (!string.IsNullOrWhiteSpace(searchTerm) && searchProperties != null && searchProperties.Length > 0)
            {
                var lowerSearch = searchTerm.ToLower();
                var predicate = string.Join(" || ", searchProperties.Select(p => $"{p}.ToLower().Contains(@0)"));
                query = query.Where(predicate, lowerSearch);
            }

            // ---------------------
            // 2. التصفية (Filtering)
            // ---------------------
            if (filters != null && filters.Count > 0)
            {
                foreach (var filter in filters)
                {
                    if (filter.Value == null) continue;

                    // exact match filter
                    query = query.Where($"{filter.Key} == @0", filter.Value);
                    Console.WriteLine(filter.Value+ "-------------"+ filter.Key);
                }
            }

            // ---------------------
            // 3. الترتيب الديناميكي
            // ---------------------
            query = ApplySortSafe(query, sortBy, defaultOrderBy: "Id");

            // ---------------------
            // 4. العد الكلي
            // ---------------------
            var totalCount = await query.CountAsync();

            // ---------------------
            // 5. الصفحات
            // ---------------------
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // ---------------------
            // 6. تحويل إلى DTOs
            // ---------------------
            var dtos = toDtoFunc(items);

            // ---------------------
            // 7. تشكيل البيانات
            // ---------------------
            var shapedItems = _dataShapingService.ShapeCollectionData(dtos, fields);

            return new PaginationResult<ExpandoObject>
            {
                Items = shapedItems,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        private IQueryable<T> ApplySortSafe<T>(IQueryable<T> query, string? sort, string defaultOrderBy)
        {
            if (string.IsNullOrWhiteSpace(sort))
                return query.OrderBy(defaultOrderBy);

            try
            {
                var validProperties = typeof(T)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(p => p.Name)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                var sortFields = sort
                    .Split(',')
                    .Select(f => f.Trim())
                    .Where(f => validProperties.Contains(f.Split(' ')[0]))
                    .Select(f =>
                    {
                        var parts = f.Split(' ');
                        var fieldName = parts[0];
                        var direction = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase)
                            ? "desc"
                            : "asc";
                        return $"{fieldName} {direction}";
                    })
                    .ToArray();

                if (sortFields.Length == 0)
                    return query.OrderBy(defaultOrderBy);

                string orderBy = string.Join(",", sortFields);
                return query.OrderBy(orderBy);
            }
            catch
            {
                return query.OrderBy(defaultOrderBy);
            }
        }
    }
}
