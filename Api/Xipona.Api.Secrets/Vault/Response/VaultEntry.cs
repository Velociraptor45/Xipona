using System.Text.Json.Serialization;

namespace ProjectHermes.Xipona.Api.Secrets.Vault.Response;

internal sealed class VaultEntry<T>
{
    [JsonPropertyName("data")]
    public required T Data { get; set; }
}