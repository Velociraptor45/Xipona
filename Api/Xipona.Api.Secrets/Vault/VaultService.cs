﻿using Polly;
using Polly.Retry;
using ProjectHermes.Xipona.Api.Secrets.Vault.Config;
using ProjectHermes.Xipona.Api.Secrets.Vault.Response;
using ProjectHermes.Xipona.Api.Secrets.Vault.Response.Token;
using System.Text.Json;

namespace ProjectHermes.Xipona.Api.Secrets.Vault;

public class VaultService : ISecretStore
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

    public Task<string?> LoadLoggingApiKey()
    {
        return Task.FromResult<string?>("");
    }

    public async Task<(string Username, string Password)> LoadDatabaseCredentialsAsync()
    {
        return await _policy.ExecuteAsync(async () =>
        {
            using var client = GetClient();
            var token = await GetToken(client);
            return await GetDbCredentials(client, token);
        });
    }

    private async Task<(string Username, string Password)> GetDbCredentials(HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Add("X-Vault-Token", token);
        var response = await client.GetAsync(
            new Uri($"v1/{_config.MountPoint}/data/{_config.Paths.Database}", UriKind.Relative));
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var deserializedResponse = JsonSerializer.Deserialize<VaultResponse<DatabaseSecret>>(content, _jsonSerializationOptions)!;
        return (deserializedResponse.Type.Data.Username, deserializedResponse.Type.Data.Password);
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
}