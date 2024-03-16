using Microsoft.Extensions.Configuration;
using ProjectHermes.ShoppingList.Api.Core.Files;
using ProjectHermes.ShoppingList.Api.Vault;
using ProjectHermes.ShoppingList.Api.Vault.Configs;

namespace ProjectHermes.ShoppingList.Api.Repositories.Common.Services;

public class DatabaseConfigurationLoadingService
{
    private readonly IFileLoadingService _fileLoadingService;
    private readonly IVaultService _vaultService;

    public DatabaseConfigurationLoadingService(IFileLoadingService fileLoadingService, IVaultService vaultService)
    {
        _fileLoadingService = fileLoadingService;
        _vaultService = vaultService;
    }

    public async Task<ConnectionStrings> LoadAsync(IConfiguration config)
    {
        var dbConfig = config.GetSection("Database").Get<DatabaseConfig>()
            ?? throw new InvalidOperationException("Database configuration is missing in appsettings");

        var usernameFile = config["PH_SL_DB_USERNAME_FILE"];
        var passwordFile = config["PH_SL_DB_PASSWORD_FILE"];

        string username;
        string password;
        if (string.IsNullOrWhiteSpace(usernameFile) || string.IsNullOrWhiteSpace(passwordFile))
        {
            (username, password) = await _vaultService.LoadCredentialsAsync();
        }
        else
        {
            username = _fileLoadingService.ReadFile(usernameFile);
            password = _fileLoadingService.ReadFile(passwordFile);
        }

        var connectionString =
            $"server={dbConfig.Address};port={dbConfig.Port};database={dbConfig.Name};user id={username};pwd={password};AllowUserVariables=true;UseAffectedRows=false";
        return new ConnectionStrings { ShoppingDatabase = connectionString };
    }

    internal class DatabaseConfig
    {
        public string Name { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public string Port { get; init; } = string.Empty;
    }
}