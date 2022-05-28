using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Infrastructure;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.WebApp.Services;

public class ConfigurationLoadingService
{
    private readonly IConfiguration _configuration;
    private readonly IFileLoadingService _fileLoadingService;

    public ConfigurationLoadingService(IConfiguration configuration, IFileLoadingService fileLoadingService)
    {
        _configuration = configuration;
        _fileLoadingService = fileLoadingService;
    }

    public async Task RegisterAsync(IServiceCollection services)
    {
        var connectionStringFile = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_FILE");

        if (connectionStringFile is null)
        {
            var vaultService = new VaultService(_configuration, _fileLoadingService);
            await vaultService.RegisterAsync(services);
        }
        else
        {
            var connectionString = _fileLoadingService.ReadFile(connectionStringFile);
            var connectionStrings = new ConnectionStrings { ShoppingDatabase = connectionString };
            services.AddSingleton(connectionStrings);
        }
    }
}