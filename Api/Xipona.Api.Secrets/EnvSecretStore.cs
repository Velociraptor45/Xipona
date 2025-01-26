using Microsoft.Extensions.Configuration;
using ProjectHermes.Xipona.Api.Core.Files;

namespace ProjectHermes.Xipona.Api.Secrets;

public class EnvSecretStore : ISecretStore
{
    private readonly IConfiguration _configuration;
    private readonly IFileLoadingService _fileLoadingService;

    public EnvSecretStore(IConfiguration configuration, IFileLoadingService fileLoadingService)
    {
        _configuration = configuration;
        _fileLoadingService = fileLoadingService;
    }

    public Task<(string Username, string Password)> LoadDatabaseCredentialsAsync()
    {
        var username = LoadSecret("PH_XIPONA_DB_USERNAME_FILE", "PH_XIPONA_DB_USERNAME");
        var password = LoadSecret("PH_XIPONA_DB_PASSWORD_FILE", "PH_XIPONA_DB_PASSWORD");

        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidOperationException("Database username is missing");
        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("Database password is missing");

        return Task.FromResult((username, password));
    }

    public Task<string?> LoadLoggingApiKey()
    {
        var apiKey = LoadSecret("PH_XIPONA_OTEL_API_KEY_FILE", "PH_XIPONA_OTEL_API_KEY");
        return Task.FromResult<string?>(apiKey);
    }

    private string LoadSecret(string envSecretFileName, string envSecretName)
    {
        var file = _configuration[envSecretFileName];
        if (string.IsNullOrWhiteSpace(file))
            return _configuration[envSecretName] ?? string.Empty;
        return _fileLoadingService.ReadFile(file);
    }
}
