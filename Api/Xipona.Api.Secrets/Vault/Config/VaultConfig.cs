using Microsoft.Extensions.Configuration;

namespace Xipona.Api.Secrets.Vault.Config;

public sealed class VaultConfig
{
    [ConfigurationKeyName("XIPONA_VAULT_URI")]
    public string Uri { get; set; } = string.Empty;

    [ConfigurationKeyName("XIPONA_VAULT_MOUNT_POINT")]
    public string MountPoint { get; set; } = string.Empty;

    [ConfigurationKeyName("XIPONA_VAULT_PATHS_DATABASE")]
    public string DatabasePath { get; set; } = string.Empty;

    [ConfigurationKeyName("XIPONA_VAULT_PATHS_LOGGING")]
    public string LoggingPath { get; set; } = string.Empty;
}