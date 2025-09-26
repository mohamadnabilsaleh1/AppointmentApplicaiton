using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Users.Errors;
using AppointmentApplication.Domain.Shared.Results;

using AppointmentApplication.Infrastructure.Authentication.Models;

using Microsoft.Extensions.Options;

namespace AppointmentApplication.Infrastructure.Authentication;

internal sealed class JwtService : IJwtService
{

    private readonly HttpClient _httpClient;
    private readonly KeycloakOptions _keycloakOptions;

    public JwtService(HttpClient httpClient, IOptions<KeycloakOptions> keycloakOptions)
    {
        _httpClient = httpClient;
        _keycloakOptions = keycloakOptions.Value;
    }

    public async Task<Result<string>> GetAccessTokenAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var authRequestParameters = new KeyValuePair<string, string>[]
        {
                new("client_id", _keycloakOptions.AuthClientId),
                new("client_secret", _keycloakOptions.AuthClientSecret),
                new("scope", "openid email"),
                new("grant_type", "password"),
                new("username", email),
                new("password", password)
        };

        using var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);

        HttpResponseMessage response = await _httpClient.PostAsync(
            "",
            authorizationRequestContent,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        AuthorizationToken? authorizationToken = await response
            .Content
            .ReadFromJsonAsync<AuthorizationToken>(cancellationToken);

        if (authorizationToken is null)
        {
            return ApplicationUserErrors.AuthenticationFailed;
        }

        return authorizationToken.AccessToken;

    }
}