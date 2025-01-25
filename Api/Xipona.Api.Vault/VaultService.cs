using Polly;
using Polly.Retry;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectHermes.Xipona.Api.Vault;

public class VaultService : IVaultService
{
    private readonly VaultCredentials _credentials;
    private readonly VaultConfig _config;

    private readonly AsyncRetryPolicy _policy;
    private readonly JsonSerializerOptions _jsonSerializationOptions;

    private const int _retryCount = 10;

    public VaultService(VaultCredentials credentials, VaultConfig config)
    {
        _credentials = credentials;
        _config = config;

        _policy = Policy.Handle<Exception>().WaitAndRetryAsync(
            _retryCount,
            i => TimeSpan.FromSeconds(Math.Pow(1.5, i) + 1),
            (e, _, tryNo, _) => Console.WriteLine($"Failed to retrieve value from vault (Try no. {tryNo}): {e}"));

        _jsonSerializationOptions = new JsonSerializerOptions();
        _jsonSerializationOptions.TypeInfoResolverChain.Add(VaultJsonSerializationContext.Default);

    }

    public async Task<(string Username, string Password)> LoadDatabaseCredentialsAsync()
    {
        return await _policy.ExecuteAsync(async () =>
        {
            using var client = GetClient();
            var token = await GetToken(client);

            //using var client2 = new HttpClient(new HttpClientHandler());
            //client2.BaseAddress = new Uri(_config.Uri);
            //var content = new StringContent()
            //client2.PostAsync($"", )

            //var result = await client.V1.Secrets.KeyValue.V2.ReadSecretAsync<DatabaseSecret>(
            //    _keyVaultConfig.Value.Paths.Database,
            //    mountPoint: _keyVaultConfig.Value.MountPoint);
            //Console.WriteLine("Successfully retrieved database credentials from vault");

            //var data = result.Data.Data;
            //return (data.Username, password: data.Password);
            return ("", "");
        });
    }

    private HttpClient GetClient()
    {
        var client = new HttpClient(new HttpClientHandler());
        client.BaseAddress = new Uri(_config.Uri);
        return client;
    }

    private async Task<string> GetToken(HttpClient client)
    {
        var data = new Dictionary<string, string>
        {
            { "password", _credentials.Password }
        };

        var requestData = new StringContent(JsonSerializer.Serialize(data, _jsonSerializationOptions));

        var response = await client.PostAsync(
            new Uri($"v1/auth/userpass/login/{_credentials.Username}", UriKind.Relative),
            requestData);

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<TokenAuthResponse>(json, _jsonSerializationOptions)!;
        return token.Auth.ClientToken;
    }

    //private IVaultClient GetClient()
    //{
    //    var authMethod = new UserPassAuthMethodInfo(_username, _password);

    //    var clientSettings = new VaultClientSettings(_keyVaultConfig.Value.Uri, authMethod);

    //    return new VaultClient(clientSettings);
    //}

    private sealed class DatabaseSecret
    {
        [JsonPropertyName("username")]
        public string Username { get; init; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; init; } = string.Empty;
    }
}

internal sealed class TokenAuthResponse
{
    [JsonPropertyName("auth")]
    public Auth Auth { get; init; } = new();
}

internal sealed class Auth
{
    [JsonPropertyName("client_token")]
    public string ClientToken { get; init; } = string.Empty;
}

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(TokenAuthResponse))]
[JsonSerializable(typeof(Auth))]
[JsonSerializable(typeof(Dictionary<string, string>))]
internal partial class VaultJsonSerializationContext : JsonSerializerContext
{
}


public sealed class VaultConfig
{
    public string Uri { get; set; } = string.Empty;
    public string MountPoint { get; set; } = string.Empty;
    public PathsConfig Paths { get; set; } = new();
}

public sealed class PathsConfig
{
    public string Database { get; set; } = string.Empty;
}

public sealed class VaultCredentials
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}