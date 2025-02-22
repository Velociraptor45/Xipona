using System.Text.Json.Serialization;

namespace ProjectHermes.Xipona.Api.Secrets.Vault.Response;

internal sealed class DatabaseSecret
{
    [JsonPropertyName("username")]
    public string Username { get; init; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; init; } = string.Empty;
}