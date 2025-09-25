using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;

using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.HealthcareFacilities;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Mappers
{
    public class HealthCareFacilitySortMapping : ISortMappingDefinition
    {
        public SortMappingDefinition<HealthCareFacility, HealthcareFacilityDto> Mapping =>
            new SortMappingDefinition<HealthCareFacility, HealthcareFacilityDto>
            {
                Mappings = new[]
                {
                    new SortMapping("name", "Name"),
                    new SortMapping("type", "Type"),
                    new SortMapping("city", "Address.City"),
                    new SortMapping("state", "Address.State"),
                    new SortMapping("country", "Address.Country"),
                    new SortMapping("zipcode", "Address.ZipCode"),
                    new SortMapping("createdat", "CreatedAtUtc"),
                    new SortMapping("isactive", "IsActive")
                }
            };
    }
}
