using Microsoft.Extensions.Configuration;
using Polly;
using ProjectHermes.ShoppingList.Api.Core.Files;
using ShoppingList.Api.Vault.Configs;
using VaultSharp;
using VaultSharp.V1.AuthMethods.UserPass;

namespace ShoppingList.Api.Vault;

public class VaultService : IVaultService
{
    private readonly string _uri;
    private readonly string _connectionStringsPath;
    private readonly string _mountPoint;
    private readonly string _password;
    private readonly string _username;

    private const int _retryCount = 10;

    public VaultService(IConfiguration configuration, IFileLoadingService fileLoadingService)
    {
        _uri = configuration["KeyVault:Uri"];
        _connectionStringsPath = configuration["KeyVault:Paths:ConnectionStrings"];
        _mountPoint = configuration["KeyVault:MountPoint"];

        var usernameFilePath = Environment.GetEnvironmentVariable("PH_SL_VAULT_USERNAME_FILE");
        var passwordFilePath = Environment.GetEnvironmentVariable("PH_SL_VAULT_PASSWORD_FILE");

        _username = string.IsNullOrWhiteSpace(usernameFilePath) ?
             string.Empty :
             fileLoadingService.ReadFile(usernameFilePath);

        _password = string.IsNullOrWhiteSpace(passwordFilePath) ?
             string.Empty :
             fileLoadingService.ReadFile(passwordFilePath);
    }

    private VaultClient GetClient()
    {
        var authMethod = new UserPassAuthMethodInfo(_username, _password);

        var clientSettings = new VaultClientSettings(_uri, authMethod);

        return new VaultClient(clientSettings);
    }

    public async Task<ConnectionStrings> LoadConnectionStringsAsync()
    {
        var policy = Policy.Handle<Exception>().WaitAndRetryAsync(
            _retryCount,
            i => TimeSpan.FromSeconds(Math.Pow(1.5, i) + 1)); // TODO #202 log exception

        return await policy.ExecuteAsync(async () =>
        {
            var client = GetClient();
            var result = await client.V1.Secrets.KeyValue.V2.ReadSecretAsync<ConnectionStrings>(
                _connectionStringsPath,
                mountPoint: _mountPoint);

            return result.Data.Data;
        });
    }
}