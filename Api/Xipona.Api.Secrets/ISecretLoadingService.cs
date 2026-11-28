using Xipona.Api.Secrets.Configs;

namespace Xipona.Api.Secrets;

public interface ISecretLoadingService
{
    Task<ConnectionStrings> LoadConnectionStringsAsync();
    Task<string?> LoadLoggingApiKey();
}