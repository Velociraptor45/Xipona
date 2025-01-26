using System.Text.Json.Serialization;

namespace ProjectHermes.Xipona.Api.Secrets.Vault.Response.Token;

internal sealed class Auth
{
    [JsonPropertyName("client_token")]
    public string ClientToken { get; init; } = string.Empty;
}