using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectHermes.Xipona.Api.Core.Files;
using ProjectHermes.Xipona.Api.Repositories.Common.Services;
using ProjectHermes.Xipona.Api.Vault;
using System.IO;

namespace ProjectHermes.Xipona.Api.Repositories.Common.Contexts;

public class ContextFactoryBase
{
    protected string GetDbConnectionString()
    {
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Xipona.Api.WebApp/"))
            .AddJsonFile($"appsettings.Local.json", optional: false, true)
            .Build();

        var fileLoadingService = new FileLoadingService();
        var configurationLoadingService = new DatabaseConfigurationLoadingService(
            fileLoadingService, new VaultService(config, fileLoadingService));

        return configurationLoadingService.LoadAsync(config).GetAwaiter().GetResult().ShoppingDatabase;
    }

    protected MySqlServerVersion GetVersion()
    {
        return new MySqlServerVersion(new Version(5, 7));
    }
}