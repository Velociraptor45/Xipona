namespace ProjectHermes.Xipona.Api.Secrets;

public interface ISecretStore
{
    Task<(string Username, string Password)> LoadDatabaseCredentialsAsync();
    Task<string?> LoadLoggingApiKey();
}
