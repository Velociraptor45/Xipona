using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.Files;
using ProjectHermes.Xipona.Api.Secrets;
using ProjectHermes.Xipona.Api.Secrets.Vault;
using ProjectHermes.Xipona.Api.Secrets.Vault.Config;

namespace ProjectHermes.Xipona.Api.WebApp.Configs;

public static class SecretStoreRegister
{
    public static void RegisterSecretStore(IConfiguration configuration, IFileLoadingService fileLoadingService,
        IServiceCollection services)
    {
        var secrets = new Secrets();
        configuration.Bind(secrets);

        if ((!string.IsNullOrWhiteSpace(secrets.VaultUsername) || !string.IsNullOrWhiteSpace(secrets.VaultUsernameFile))
            && (!string.IsNullOrWhiteSpace(secrets.VaultPassword) || !string.IsNullOrWhiteSpace(secrets.VaultPasswordFile)))
        {
            // use vault
            Console.WriteLine("Found Vault username & password configuration. Using Vault as secret store.");

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

            services.AddSingleton(vaultConfig);
            services.AddSingleton(vaultCredentials);
            services.AddTransient<ISecretStore, VaultService>();
            services.AddHttpClient("vault", client => client.BaseAddress = new Uri(vaultConfig.Uri));
        }
        else
        {
            // use env
            Console.WriteLine("Couldn't find Vault username & password configuration. Using environment variables as secret store.");
            services.AddTransient<ISecretStore, EnvSecretStore>();
        }
    }
}

internal class Secrets
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
