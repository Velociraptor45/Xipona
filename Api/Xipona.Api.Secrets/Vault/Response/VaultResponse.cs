using System.Text.Json.Serialization;

namespace ProjectHermes.Xipona.Api.Secrets.Vault.Response;

internal sealed class VaultResponse<T>
{
    [JsonPropertyName("data")]
    public required VaultEntry<T> Type { get; set; }
}