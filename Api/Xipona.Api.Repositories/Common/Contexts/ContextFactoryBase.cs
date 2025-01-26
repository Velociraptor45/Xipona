using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.Files;
using ProjectHermes.Xipona.Api.Secrets;
using System.IO;

namespace ProjectHermes.Xipona.Api.Repositories.Common.Contexts;

/// <summary>
/// This is needed for creating migrations.
/// </summary>
public abstract class ContextFactoryBase
{
    protected static string GetDbConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Xipona.Api.WebApp/"))
            .AddJsonFile("appsettings.Local.json", optional: false, true)
            .Build();

        var secretServices = new ServiceCollection();
        secretServices.AddSingleton<IConfiguration>(configuration);
        secretServices.AddTransient<IFileLoadingService, FileLoadingService>();
        SecretStoreRegister.RegisterSecretStore(configuration, new FileLoadingService(), secretServices);
        secretServices.AddTransient<ISecretLoadingService, SecretLoadingService>();

        var secretProvider = secretServices.BuildServiceProvider();

        var secretLoadingService = secretProvider.GetRequiredService<ISecretLoadingService>();
        var connectionStrings = secretLoadingService.LoadConnectionStringsAsync().GetAwaiter().GetResult();
        return connectionStrings.ShoppingDatabase;
    }

    protected static MySqlServerVersion GetVersion()
    {
        return new MySqlServerVersion(new Version(5, 7));
    }
}