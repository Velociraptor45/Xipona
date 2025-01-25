using Microsoft.Extensions.Configuration;
using ProjectHermes.Xipona.Api.Core.Files;
using ProjectHermes.Xipona.Api.Repositories.Common.Services;
using ProjectHermes.Xipona.Api.Repositories.Configs;
using ProjectHermes.Xipona.Api.Secrets;
using ProjectHermes.Xipona.Api.Secrets.Vault;
using System.Threading.Tasks;

namespace ProjectHermes.Xipona.Api.WebApp.Configs;
public class SecretRetriever : ISecretRetriever
{
    private readonly IConfiguration _configuration;
    private readonly ISecretStore _secretStore;

    public SecretRetriever(IConfiguration configuration, IFileLoadingService fileLoadingService)
    {
        _configuration = configuration;

        var secrets = new Secrets();
        configuration.Bind(secrets);

        if ((!string.IsNullOrWhiteSpace(secrets.VaultUsername) || !string.IsNullOrWhiteSpace(secrets.VaultUsernameFile))
            && (!string.IsNullOrWhiteSpace(secrets.VaultPassword) || !string.IsNullOrWhiteSpace(secrets.VaultPasswordFile)))
        {
            // use vault
            var vaultCredentials = new VaultCredentials
            {
                Username = !string.IsNullOrWhiteSpace(secrets.VaultUsername)
                    ? secrets.VaultUsername
                    : fileLoadingService.ReadFile(secrets.VaultUsernameFile),
                Password = !string.IsNullOrWhiteSpace(secrets.VaultPassword)
                    ? secrets.VaultPassword
                    : fileLoadingService.ReadFile(secrets.VaultPasswordFile)
            };

            var vaultConfig = new VaultConfig();
            configuration.GetSection("KeyVault").Bind(vaultConfig, opt => opt.ErrorOnUnknownConfiguration = true);

            _secretStore = new VaultService(vaultCredentials, vaultConfig);
        }
        else
        {
            // use env
            _secretStore = new EnvSecretStore(configuration, fileLoadingService);
        }
    }

    public async Task<ConnectionStrings> LoadDatabaseCredentialsAsync()
    {
        var credentials = await _secretStore.LoadDatabaseCredentialsAsync();
        var connectionStrings = new DatabaseConfigurationLoadingService(_configuration)
            .GetConnectionString(credentials.Username, credentials.Password);
        return connectionStrings;
    }

    public Task<string?> LoadLoggingApiKey()
    {
        return _secretStore.LoadLoggingApiKey();
    }

    class Secrets
    {
        [ConfigurationKeyName("PH_XIPONA_VAULT_USERNAME")]
        public string VaultUsername { get; set; } = string.Empty;

        [ConfigurationKeyName("PH_XIPONA_VAULT_USERNAME_FILE")]
        public string VaultUsernameFile { get; set; } = string.Empty;

        [ConfigurationKeyName("PH_XIPONA_VAULT_PASSWORD")]
        public string VaultPassword { get; set; } = string.Empty;

        [ConfigurationKeyName("PH_XIPONA_VAULT_PASSWORD_FILE")]
        public string VaultPasswordFile { get; set; } = string.Empty;
    }
}
