using ProjectHermes.Xipona.Api.Repositories.Configs;
using System.Threading.Tasks;

namespace ProjectHermes.Xipona.Api.WebApp.Configs;

public interface ISecretRetriever
{
    Task<ConnectionStrings> LoadDatabaseCredentialsAsync();
    Task<string?> LoadLoggingApiKey();
}