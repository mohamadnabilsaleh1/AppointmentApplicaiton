using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AppointmentApplication.Application.Features.HealthcareFacilities.Mappers;
using AppointmentApplication.Application.Shared.Query;

using AppointmentApplication.Application.Shared.Services;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            // cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            // cfg.AddOpenBehavior(typeof(PerformanceBehaviour<,>));
            // cfg.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));
            // cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
        });

        // Register all ISortMappingDefinition implementations
        services.Scan(scan => scan
            .FromAssemblyOf<SortMappingProvider>() // or your application assembly
            .AddClasses(classes => classes.AssignableTo<ISortMappingDefinition>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        // Register SortMappingProvider itself
        services.AddSingleton<SortMappingProvider>();
        services.AddTransient<DataShapingService>();
        services.AddSingleton<ISortMappingDefinition, HealthCareFacilitySortMapping>();
        services.AddTransient<DynamicQueryService>();
        return services;
    }

}



