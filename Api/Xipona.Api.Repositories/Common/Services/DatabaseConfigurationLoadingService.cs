using Microsoft.Extensions.Configuration;
using ProjectHermes.Xipona.Api.Core.Files;
using ProjectHermes.Xipona.Api.Vault;
using ProjectHermes.Xipona.Api.Vault.Configs;

namespace ProjectHermes.Xipona.Api.Repositories.Common.Services;

public class DatabaseConfigurationLoadingService
{
    private readonly IFileLoadingService _fileLoadingService;
    private readonly IVaultService _vaultService;
    private readonly IConfiguration _configuration;

    public DatabaseConfigurationLoadingService(IFileLoadingService fileLoadingService, IVaultService vaultService,
        IConfiguration configuration)
    {
        _fileLoadingService = fileLoadingService;
        _vaultService = vaultService;
        _configuration = configuration;
    }

    public async Task<ConnectionStrings> LoadAsync()
    {
        var dbConfig = new DatabaseConfig();
        _configuration.GetSection("Database").Bind(dbConfig, opt => opt.ErrorOnUnknownConfiguration = true);

        string username = LoadSecret("PH_XIPONA_DB_USERNAME_FILE", "PH_XIPONA_DB_USERNAME");
        string password = LoadSecret("PH_XIPONA_DB_PASSWORD_FILE", "PH_XIPONA_DB_PASSWORD");

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            (username, password) = await _vaultService.LoadDatabaseCredentialsAsync();
        }

        var connectionString =
            $"server={dbConfig.Address};port={dbConfig.Port};database={dbConfig.Name};user id={username};pwd={password};AllowUserVariables=true;UseAffectedRows=false";
        return new ConnectionStrings { ShoppingDatabase = connectionString };
    }

    private string LoadSecret(string envSecretFileName, string envSecretName)
    {
        var file = _configuration[envSecretFileName];
        if (string.IsNullOrWhiteSpace(file))
            return _configuration[envSecretName] ?? string.Empty;

        return _fileLoadingService.ReadFile(file);
    }

    internal class DatabaseConfig
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
    }
}