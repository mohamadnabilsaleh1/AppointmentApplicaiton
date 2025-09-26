using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppointmentApplication.Infrastructure.Authentication.Models;

internal sealed class AuthorizationToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = string.Empty;
}