using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Users;
using AppointmentApplication.Infrastructure.Authentication;
using AppointmentApplication.Infrastructure.Authorization;
using AppointmentApplication.Infrastructure.Data;
using AppointmentApplication.Infrastructure.Services;

using Microsoft.AspNetCore.Authentication;

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
        string? countryConnectionString = configuration.GetConnectionString("CountryConnection");

        ArgumentNullException.ThrowIfNull(connectionString);
        ArgumentNullException.ThrowIfNull(countryConnectionString);

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddScoped<IFileStorageService, FileStorageService>();

        services.AddDbContext<CountryUsersDbContext>(options => options.UseSqlServer(countryConnectionString));
        services.AddScoped<ICountryUsersDbContext>(provider => provider.GetRequiredService<CountryUsersDbContext>());

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer();
        services.AddTransient<AdminAuthorizationDelegatingHandler>();

        services.Configure<Authentication.AuthenticationOptions>(configuration.GetSection("Authentication"));
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));
        services.AddHttpClient<Application.Abstractions.Authentication.IAuthenticationService, Authentication.AuthenticationService>((serviceProvider, httpClient) =>
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
        AddAuthorization(services);
        services.AddTransient<IUserContext, UserContext>();

        return services;
    }

    private static void AddAuthorization(IServiceCollection services)
    {
        services.AddScoped<AuthorizationService>();
        services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();
    }
}