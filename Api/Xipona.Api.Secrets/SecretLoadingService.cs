using Microsoft.Extensions.Configuration;
using ProjectHermes.Xipona.Api.Secrets.Configs;

namespace ProjectHermes.Xipona.Api.Secrets;

public class SecretLoadingService : ISecretLoadingService
{
    private readonly IConfiguration _configuration;
    private readonly ISecretStore _secretStore;

    public SecretLoadingService(IConfiguration configuration, ISecretStore secretStore)
    {
        _configuration = configuration;
        _secretStore = secretStore;
    }

    public async Task<ConnectionStrings> LoadConnectionStringsAsync()
    {
        var credentials = await _secretStore.LoadDatabaseCredentialsAsync();
        var connectionStrings = GetConnectionString(credentials.Username, credentials.Password);
        return connectionStrings;
    }

    public Task<string?> LoadLoggingApiKey()
    {
        return _secretStore.LoadLoggingApiKey();
    }

    private ConnectionStrings GetConnectionString(string username, string password)
    {
        var dbConfig = new DatabaseConfig();
        _configuration.GetSection("Database").Bind(dbConfig, opt => opt.ErrorOnUnknownConfiguration = true);

        var connectionString =
            $"server={dbConfig.Address};port={dbConfig.Port};database={dbConfig.Name};user id={username};pwd={password};AllowUserVariables=true;UseAffectedRows=false";

        return new ConnectionStrings { ShoppingDatabase = connectionString };
    }

    internal class DatabaseConfig
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
    }
}