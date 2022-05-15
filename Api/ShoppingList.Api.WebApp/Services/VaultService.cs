using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Infrastructure;
using System.Threading.Tasks;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace ProjectHermes.ShoppingList.Api.WebApp.Services;

public class VaultService
{
    private readonly string _token;
    private readonly string _uri;
    private readonly string _connectionStringsPath;
    private VaultClient _client;

    public VaultService(IConfiguration configuration)
    {
        _token = configuration["KeyVault:Token"];
        _uri = configuration["KeyVault:Uri"];
        _connectionStringsPath = configuration["KeyVault:Paths:ConnectionStrings"];
    }

    private VaultClient GetClient()
    {
        if (_client is not null)
            return _client;

        var authMethod = new TokenAuthMethodInfo(_token);

        var clientSettings = new VaultClientSettings(_uri, authMethod);

        _client = new VaultClient(clientSettings);

        return _client;
    }

    private async Task RegisterConnectionStringsAsync(IServiceCollection services)
    {
        var client = _client ?? GetClient();

        var result = await client.V1.Secrets.KeyValue.V2.ReadSecretAsync<ConnectionStrings>(_connectionStringsPath);

        var connectionStrings = result.Data.Data;

        services.AddSingleton(connectionStrings);
    }

    public async Task RegisterAsync(IServiceCollection services)
    {
        await RegisterConnectionStringsAsync(services);
    }
}