using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;
using ProjectHermes.ShoppingList.Api.Core.Files;
using VaultSharp;
using VaultSharp.V1.AuthMethods.UserPass;

namespace ProjectHermes.ShoppingList.Api.Vault;

public class VaultService : IVaultService
{
    private readonly string _password;
    private readonly string _username;
    private readonly Lazy<KeyVaultConfig> _keyVaultConfig;
    private readonly AsyncRetryPolicy _policy;

    private const int _retryCount = 10;

    public VaultService(IConfiguration configuration, IFileLoadingService fileLoadingService)
    {
        _keyVaultConfig = new Lazy<KeyVaultConfig>(() =>
            configuration.GetSection("KeyVault").Get<KeyVaultConfig>()
                ?? throw new InvalidOperationException("KeyVault config is missing in appsettings"));

        var usernameFilePath = Environment.GetEnvironmentVariable("PH_SL_VAULT_USERNAME_FILE");
        var passwordFilePath = Environment.GetEnvironmentVariable("PH_SL_VAULT_PASSWORD_FILE");

        _username = string.IsNullOrWhiteSpace(usernameFilePath) ?
             string.Empty :
             fileLoadingService.ReadFile(usernameFilePath);

        _password = string.IsNullOrWhiteSpace(passwordFilePath) ?
             string.Empty :
             fileLoadingService.ReadFile(passwordFilePath);

        _policy = Policy.Handle<Exception>().WaitAndRetryAsync(
            _retryCount,
            i => TimeSpan.FromSeconds(Math.Pow(1.5, i) + 1),
            (e, _, tryNo, _) => Console.WriteLine($"Failed to retrieve value from vault (Try no. {tryNo}): {e}"));
    }

    public async Task<(string Username, string Password)> LoadCredentialsAsync()
    {
        var userName = await ReadValueAsync(_keyVaultConfig.Value.Paths.Username);
        Console.WriteLine("Successfully retrieved user name from vault");
        var password = await ReadValueAsync(_keyVaultConfig.Value.Paths.Password);
        Console.WriteLine("Successfully retrieved password from vault");

        return (userName, password);
    }

    private VaultClient GetClient()
    {
        var authMethod = new UserPassAuthMethodInfo(_username, _password);

        var clientSettings = new VaultClientSettings(_keyVaultConfig.Value.Uri, authMethod);

        return new VaultClient(clientSettings);
    }

    private async Task<string> ReadValueAsync(string path)
    {
        return await _policy.ExecuteAsync(async () =>
        {
            var client = GetClient();
            var result = await client.V1.Secrets.KeyValue.V2.ReadSecretAsync<string>(
                path,
                mountPoint: _keyVaultConfig.Value.MountPoint);

            return result.Data.Data;
        });
    }

    private class KeyVaultConfig
    {
        public string Uri { get; init; } = string.Empty;
        public string MountPoint { get; init; } = string.Empty;
        public PathsConfig Paths { get; init; } = new();
    }

    private class PathsConfig
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}