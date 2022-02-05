using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Common.Contexts;

public class ContextFactoryBase
{
    protected string GetDbConnectionString()
    {
        string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);

        if (env is null)
            throw new InvalidOperationException("Environment variable 'ASPNETCORE_ENVIRONMENT' not found.");

        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ShoppingList.Api.WebApp/"))
            .AddJsonFile($"appsettings.{env}.json", optional: false, true)
            .Build();

        return config["ConnectionStrings:Shopping-Database"];
    }

    protected MySqlServerVersion GetVersion()
    {
        var version = Assembly.GetEntryAssembly()!.GetName().Version!;
        return new MySqlServerVersion(new Version(version.Major, version.Minor, version.Build));
    }
}