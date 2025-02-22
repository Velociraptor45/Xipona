using ProjectHermes.Xipona.Api.Secrets.Configs;

namespace ProjectHermes.Xipona.Api.Secrets;

public interface ISecretLoadingService
{
    Task<ConnectionStrings> LoadConnectionStringsAsync();
    Task<string?> LoadLoggingApiKey();
}