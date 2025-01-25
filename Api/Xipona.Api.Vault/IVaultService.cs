namespace ProjectHermes.Xipona.Api.Vault;

public interface IVaultService
{
    Task<(string Username, string Password)> LoadDatabaseCredentialsAsync();
}