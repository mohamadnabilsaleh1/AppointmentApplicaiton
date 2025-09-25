using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace AppointmentApplication.Application.Shared.Services
{
    public static class DynamicSorter
    {
        /// <summary>
        /// ترتيب ديناميكي لأي IQueryable مع التحقق من الحقول وإرجاع ترتيب افتراضي إذا حدث خطأ
        /// </summary>
        /// <typeparam name="T">نوع الـ Entity أو DTO</typeparam>
        /// <param name="query">الـ IQueryable</param>
        /// <param name="sort">سلسلة الترتيب مثل "Name desc, CreatedAt asc"</param>
        /// <param name="defaultOrderBy">حقل افتراضي للترتيب إذا حدث خطأ</param>
        /// <returns>IQueryable مرتبة</returns>
        public static IQueryable<T> ApplySortSafe<T>(this IQueryable<T> query, string? sort, string defaultOrderBy = "Id")
        {
            if (string.IsNullOrWhiteSpace(sort))
            {

                return query.OrderBy(defaultOrderBy);
            }


            try
            {
                // جمع كل الحقول الصالحة
                var validProperties = typeof(T)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(p => p.Name)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                // فصل الحقول والتحقق من كل حقل
                var sortFields = sort
                    .Split(',')
                    .Select(f => f.Trim())
                    .Where(f => validProperties.Contains(f.Split(' ')[0])) // فقط الحقول الموجودة
                    .Select(f =>
                    {
                        var parts = f.Split(' ');
                        var fieldName = parts[0];
                        var direction = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase)
                            ? "desc"
                            : "asc"; // أي قيمة غير صحيحة تتحول إلى asc
                        return $"{fieldName} {direction}";
                    })
                    .ToArray();

                if (sortFields.Length == 0)
                {

                    return query.OrderBy(defaultOrderBy);
                }


                string orderBy = string.Join(",", sortFields);
                return query.OrderBy(orderBy);
            }
            catch
            {
                // fallback إذا حصل أي خطأ
                return query.OrderBy(defaultOrderBy);
            }
        }
    }
}
