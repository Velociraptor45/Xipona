namespace ProjectHermes.Xipona.Api.Secrets.Vault;

public interface IVaultService
{
    Task<(string Username, string Password)> LoadDatabaseCredentialsAsync();
}