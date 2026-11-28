using Polly;
using Polly.Retry;
using System.Text.Json;
using Xipona.Api.Secrets.Vault.Config;
using Xipona.Api.Secrets.Vault.Response;
using Xipona.Api.Secrets.Vault.Response.Token;

namespace Xipona.Api.Secrets.Vault;

public class VaultService : ISecretStore
{
    private readonly VaultCredentials _credentials;
    private readonly VaultConfig _config;
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly ResiliencePipeline _pipeline;
    private readonly JsonSerializerOptions _jsonSerializationOptions;

    private const int _retryCount = 10;

    public VaultService(VaultCredentials credentials, VaultConfig config, IHttpClientFactory httpClientFactory)
    {
        _credentials = credentials;
        _config = config;
        _httpClientFactory = httpClientFactory;

        var retryOpt = new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>(),
            MaxRetryAttempts = _retryCount,
            Delay = TimeSpan.FromSeconds(5),
            OnRetry = static args =>
            {
                Console.WriteLine($"Failed to retrieve value from vault (Try no. {args.AttemptNumber}): {args.Outcome.Exception}");

                return default;
            }
        };
        _pipeline = new ResiliencePipelineBuilder().AddRetry(retryOpt).Build();


        _jsonSerializationOptions = new JsonSerializerOptions();
        _jsonSerializationOptions.TypeInfoResolverChain.Add(VaultJsonSerializationContext.Default);
    }

    public async Task<string?> LoadLoggingApiKey()
    {
        if (string.IsNullOrWhiteSpace(_config.LoggingPath))
            return null;

        return await _pipeline.ExecuteAsync(async ct =>
        {
            var loggingSecret = await GetSecret<LoggingSecret>(_config.LoggingPath, ct);
            return loggingSecret.ApiKey;
        });
    }

    public async Task<(string Username, string Password)> LoadDatabaseCredentialsAsync()
    {
        return await _pipeline.ExecuteAsync(async ct =>
        {
            var dbSecret = await GetSecret<DatabaseSecret>(_config.DatabasePath, ct);
            return (dbSecret.Username, dbSecret.Password);
        });
    }

    private async Task<T> GetSecret<T>(string secret, CancellationToken cancellationToken)
    {
        using var client = _httpClientFactory.CreateClient("vault");
        var token = await GetToken(client, cancellationToken);

        client.DefaultRequestHeaders.Add("X-Vault-Token", token);
        var response = await client.GetAsync(
            new Uri($"v1/{_config.MountPoint}/data/{secret}", UriKind.Relative),
            cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var deserializedResponse = JsonSerializer.Deserialize<VaultResponse<T>>(content, _jsonSerializationOptions)!;
        return deserializedResponse.Type.Data;
    }

    private async Task<string> GetToken(HttpClient client, CancellationToken cancellationToken)
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
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var token = JsonSerializer.Deserialize<TokenAuthResponse>(json, _jsonSerializationOptions)!;
        return token.Auth.ClientToken;
    }
}