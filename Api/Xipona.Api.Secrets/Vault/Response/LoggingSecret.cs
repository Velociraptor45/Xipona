using System.Text.Json.Serialization;

namespace Xipona.Api.Secrets.Vault.Response;
internal sealed class LoggingSecret
{
    [JsonPropertyName("apiKey")]
    public string ApiKey { get; init; } = string.Empty;
}
