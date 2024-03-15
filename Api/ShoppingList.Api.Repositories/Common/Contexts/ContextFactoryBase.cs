using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectHermes.ShoppingList.Api.Core.Files;
using ProjectHermes.ShoppingList.Api.Repositories.Common.Services;
using ProjectHermes.ShoppingList.Api.Vault;
using System.IO;

namespace ProjectHermes.ShoppingList.Api.Repositories.Common.Contexts;

public class ContextFactoryBase
{
    protected string GetDbConnectionString()
    {
        string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);

        if (env is null)
            throw new InvalidOperationException("Environment variable 'ASPNETCORE_ENVIRONMENT' not found.");

        if (string.IsNullOrWhiteSpace(env))
            throw new InvalidOperationException("Environment variable 'ASPNETCORE_ENVIRONMENT' is empty.");

        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ShoppingList.Api.WebApp/"))
            .AddJsonFile($"appsettings.{env}.json", optional: false, true)
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