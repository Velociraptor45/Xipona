namespace ProjectHermes.Xipona.Api.Secrets.Vault.Config;

public sealed class VaultConfig
{
    public string Uri { get; set; } = string.Empty;
    public string MountPoint { get; set; } = string.Empty;
    public PathsConfig Paths { get; set; } = new();
}