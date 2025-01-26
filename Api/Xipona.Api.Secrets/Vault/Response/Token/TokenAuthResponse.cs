using System.Text.Json.Serialization;

namespace ProjectHermes.Xipona.Api.Secrets.Vault.Response.Token;

internal sealed class TokenAuthResponse
{
    [JsonPropertyName("auth")]
    public Auth Auth { get; init; } = new();
}