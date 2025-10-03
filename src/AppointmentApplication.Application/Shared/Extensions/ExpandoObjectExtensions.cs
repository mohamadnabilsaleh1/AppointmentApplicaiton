using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Application.Shared.Extensions
{
    public static class ExpandoObjectExtensions
    {
        public static object? GetPropertyValue(this ExpandoObject expando, string propertyName)
        {
            var dict = (IDictionary<string, object?>)expando;
            dict.TryGetValue(propertyName, out var value);
            return value;
        }
    }
}