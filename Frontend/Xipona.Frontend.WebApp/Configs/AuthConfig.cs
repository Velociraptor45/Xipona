namespace ProjectHermes.Xipona.Frontend.WebApp.Configs;

public sealed class AuthConfig
{
    public bool Enabled { get; init; } = false;
    public string UserRoleName { get; init; } = "User";
}