using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        return services;
    }
}



