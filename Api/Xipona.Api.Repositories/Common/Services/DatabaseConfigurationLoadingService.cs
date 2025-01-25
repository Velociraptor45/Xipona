using Microsoft.Extensions.Configuration;
using ProjectHermes.Xipona.Api.Repositories.Configs;

namespace ProjectHermes.Xipona.Api.Repositories.Common.Services;

public class DatabaseConfigurationLoadingService
{
    private readonly IConfiguration _configuration;

    public DatabaseConfigurationLoadingService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ConnectionStrings GetConnectionString(string username, string password)
    {
        var dbConfig = new DatabaseConfig();
        _configuration.GetSection("Database").Bind(dbConfig, opt => opt.ErrorOnUnknownConfiguration = true);

        var connectionString =
            $"server={dbConfig.Address};port={dbConfig.Port};database={dbConfig.Name};user id={username};pwd={password};AllowUserVariables=true;UseAffectedRows=false";

        return new ConnectionStrings() { ShoppingDatabase = connectionString };
    }

    internal class DatabaseConfig
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
    }
}