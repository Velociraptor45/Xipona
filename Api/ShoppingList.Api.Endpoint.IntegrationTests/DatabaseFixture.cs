using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.ApplicationServices;
using ProjectHermes.ShoppingList.Api.Domain;
using ProjectHermes.ShoppingList.Api.Endpoint;
using ProjectHermes.ShoppingList.Api.Infrastructure;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;
using System;

namespace ShoppingList.Api.Endpoint.IntegrationTests;

public abstract class DatabaseFixture
{
    protected readonly DockerFixture DockerFixture;
    private readonly IServiceProvider _provider;

    protected DatabaseFixture(DockerFixture dockerFixture)
    {
        DockerFixture = dockerFixture;

        _provider = CreateServiceProvider();
    }

    protected IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddDomain();
        services.AddEndpointControllers();
        services.AddInfrastructure(DockerFixture.ConnectionString);
        services.AddApplicationServices();

        return services.BuildServiceProvider();
    }

    public IServiceScope CreateServiceScope()
    {
        return _provider.CreateScope();
    }

    public async Task ApplyMigrationsAsync(IServiceScope scope)
    {
        var contexts = GetDbContexts(scope).ToList();

        await contexts.First().Database.ExecuteSqlRawAsync(
            "DROP TABLE IF EXISTS ItemCategories;" +
            "DROP TABLE IF EXISTS Manufacturers;" +
            "DROP TABLE IF EXISTS AvailableAts;" +
            "DROP TABLE IF EXISTS ItemTypeAvailableAts;" +
            "DROP TABLE IF EXISTS ItemTypes;" +
            "DROP TABLE IF EXISTS Sections;" +
            "DROP TABLE IF EXISTS Stores;" +
            "DROP TABLE IF EXISTS ItemsOnLists;" +
            "DROP TABLE IF EXISTS Items;" +
            "DROP TABLE IF EXISTS ShoppingLists;" +
            "DROP TABLE IF EXISTS `__EFMigrationsHistory`;");

        foreach (DbContext context in contexts)
        {
            await context.Database.MigrateAsync();
        }
    }

    public abstract IEnumerable<DbContext> GetDbContexts(IServiceScope scope);

    public async Task<ITransaction> CreateTransactionAsync(IServiceScope scope)
    {
        var generator = scope.ServiceProvider.GetRequiredService<ITransactionGenerator>();
        return await generator.GenerateAsync(default);
    }
}