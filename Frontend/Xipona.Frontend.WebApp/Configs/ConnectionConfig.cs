using Microsoft.Extensions.Configuration;

namespace Xipona.Frontend.WebApp.Configs;

public sealed class ConnectionConfig
{
    [ConfigurationKeyName("XIPONA_API_URL")]
    public string ApiUri { get; set; } = string.Empty;
}