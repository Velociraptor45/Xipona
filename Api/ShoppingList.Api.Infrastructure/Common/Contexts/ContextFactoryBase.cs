using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Common.Contexts;

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

        return config["ConnectionStrings:Shopping-Database"];
    }

    protected MySqlServerVersion GetVersion()
    {
        return new MySqlServerVersion(new Version(5, 7));
    }
}