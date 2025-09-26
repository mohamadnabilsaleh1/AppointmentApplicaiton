using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Infrastructure.Authentication;
using AppointmentApplication.Infrastructure.Data;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AppointmentApplication.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);

        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer();
        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));
        services.AddHttpClient<IAuthenticationService, AuthenticationService>((serviceProvider, httpClient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
            httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
        })
        .AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();
        services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
            httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
        });
        return services;
    }
}