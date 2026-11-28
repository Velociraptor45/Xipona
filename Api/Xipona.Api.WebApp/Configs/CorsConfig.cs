using Microsoft.Extensions.Configuration;

namespace Xipona.Api.WebApp.Configs;

internal class CorsConfig
{
    [ConfigurationKeyName("XIPONA_CORS_ORIGIN")]
    public string[] AllowedOrigins { get; set; } = [];
}