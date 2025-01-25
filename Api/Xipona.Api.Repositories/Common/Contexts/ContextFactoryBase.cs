using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ProjectHermes.Xipona.Api.Repositories.Common.Contexts;

public class ContextFactoryBase
{
    protected string GetDbConnectionString()
    {
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Xipona.Api.WebApp/"))
            .AddJsonFile("appsettings.Local.json", optional: false, true)
            .Build();

        return ""; // todo
        //var fileLoadingService = new FileLoadingService();
        //var configurationLoadingService = new DatabaseConfigurationLoadingService(
        //    fileLoadingService, new VaultService(config, fileLoadingService), config);

        //return configurationLoadingService.LoadAsync().GetAwaiter().GetResult().ShoppingDatabase;
    }

    protected MySqlServerVersion GetVersion()
    {
        return new MySqlServerVersion(new Version(5, 7));
    }
}