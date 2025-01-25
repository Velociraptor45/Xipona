using ProjectHermes.Xipona.Api.Secrets.Vault.Response;
using ProjectHermes.Xipona.Api.Secrets.Vault.Response.Token;
using System.Text.Json.Serialization;

namespace ProjectHermes.Xipona.Api.Secrets.Vault;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(TokenAuthResponse))]
[JsonSerializable(typeof(Auth))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(VaultResponse<DatabaseSecret>))]
[JsonSerializable(typeof(VaultEntry<DatabaseSecret>))]
[JsonSerializable(typeof(DatabaseSecret))]
internal partial class VaultJsonSerializationContext : JsonSerializerContext
{
}