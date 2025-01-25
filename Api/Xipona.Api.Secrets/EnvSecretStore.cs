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
        throw new NotImplementedException();
    }

    public Task<string?> LoadLoggingApiKey()
    {
        throw new NotImplementedException();
    }

    private string LoadSecret(string envSecretFileName, string envSecretName)
    {
        var file = _configuration[envSecretFileName];
        if (string.IsNullOrWhiteSpace(file))
            return _configuration[envSecretName] ?? string.Empty;
        return _fileLoadingService.ReadFile(file);
    }
}
