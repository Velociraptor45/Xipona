using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ProjectHermes.ShoppingList.Api.ApplicationServices;
using ProjectHermes.ShoppingList.Api.Core;
using ProjectHermes.ShoppingList.Api.Domain;
using ProjectHermes.ShoppingList.Api.Infrastructure;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.IntegrationTests;

public abstract class DatabaseFixture : IDisposable
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
        services.AddCore();
        services.AddDomain();
        services.AddEndpointControllers();
        services.AddInfrastructure(DockerFixture.ConnectionString);
        services.AddApplicationServices();

        services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));

        return services.BuildServiceProvider();
    }

    public IServiceScope CreateServiceScope()
    {
        return _provider.CreateScope();
    }

    public async Task ApplyMigrationsAsync(IServiceScope scope)
    {
        var contexts = GetDbContexts(scope).ToList();

        await contexts.First().Database.EnsureDeletedAsync();

        foreach (DbContext context in contexts)
        {
            await context.Database.MigrateAsync();
        }
    }

    public abstract IEnumerable<DbContext> GetDbContexts(IServiceScope scope);

    protected TContext GetContextInstance<TContext>(IServiceScope scope) where TContext : DbContext
    {
        return scope.ServiceProvider.GetRequiredService<TContext>();
    }

    public async Task<ITransaction> CreateTransactionAsync(IServiceScope scope)
    {
        var generator = scope.ServiceProvider.GetRequiredService<ITransactionGenerator>();
        return await generator.GenerateAsync(default);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            DockerFixture.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}