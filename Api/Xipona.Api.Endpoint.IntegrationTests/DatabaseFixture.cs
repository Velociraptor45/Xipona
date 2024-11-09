using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MySqlConnector;
using ProjectHermes.Xipona.Api.ApplicationServices;
using ProjectHermes.Xipona.Api.Core;
using ProjectHermes.Xipona.Api.Domain;
using ProjectHermes.Xipona.Api.Repositories;
using ProjectHermes.Xipona.Api.Repositories.Common.Transactions;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Contexts;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Entities;
using ProjectHermes.Xipona.Api.Repositories.Items.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Items.Entities;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Entities;
using ProjectHermes.Xipona.Api.Repositories.RecipeTags.Contexts;
using ProjectHermes.Xipona.Api.Repositories.RecipeTags.Entities;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Stores.Contexts;
using System;

namespace ProjectHermes.Xipona.Api.Endpoint.IntegrationTests;

public abstract class DatabaseFixture : IDisposable
{
    private static readonly object _lock = new();
    protected readonly DockerFixture DockerFixture;
    private readonly IServiceProvider _provider;
    private static int _databaseNameCounter = 0;
    private readonly string _connectionString;

    protected DatabaseFixture(DockerFixture dockerFixture)
    {
        DockerFixture = dockerFixture;

        string databaseName;
        lock (_lock)
        {
            _databaseNameCounter++;
            databaseName = $"{DockerFixture.DatabaseName}-{_databaseNameCounter}";
            _connectionString = DockerFixture.ConnectionString.Replace("{DatabaseName}", databaseName);
        }

        using var connection = new MySqlConnection(DockerFixture.ConnectionStringWithoutDb);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{databaseName}`;";
        cmd.ExecuteNonQuery();

        connection.Close();

        _provider = CreateServiceProvider();
    }

    protected IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddCore();
        services.AddDomain();
        services.AddEndpointControllers();
        services.AddRepositories(_connectionString);
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

        foreach (DbContext context in contexts)
        {
            await context.Database.MigrateAsync();
        }
    }

    public abstract IEnumerable<DbContext> GetDbContexts(IServiceScope scope);

    public TContext GetContextInstance<TContext>(IServiceScope scope) where TContext : DbContext
    {
        return scope.ServiceProvider.GetRequiredService<TContext>();
    }

    public static async Task<ITransaction> CreateTransactionAsync(IServiceScope scope)
    {
        var generator = scope.ServiceProvider.GetRequiredService<ITransactionGenerator>();
        return await generator.GenerateAsync(default);
    }

    public async Task<IEnumerable<Recipe>> LoadAllRecipesAsync(IServiceScope assertionScope)
    {
        await using var recipeContext = GetContextInstance<RecipeContext>(assertionScope);

        return await recipeContext.Recipes.AsNoTracking()
            .Include(r => r.Ingredients)
            .Include(r => r.PreparationSteps)
            .Include(r => r.Tags)
            .ToListAsync();
    }

    public async Task<IEnumerable<ItemCategory>> LoadAllItemCategoriesAsync(IServiceScope assertionScope)
    {
        await using var itemCategoryContext = GetContextInstance<ItemCategoryContext>(assertionScope);

        return await itemCategoryContext.ItemCategories.AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Item>> LoadAllItemsAsync(IServiceScope assertionScope)
    {
        await using var itemContext = GetContextInstance<ItemContext>(assertionScope);

        return await itemContext.Items.AsNoTracking()
            .Include(item => item.AvailableAt)
            .Include(item => item.ItemTypes)
            .ThenInclude(itemType => itemType.AvailableAt)
            .Include(item => item.ItemTypes)
            .ToListAsync();
    }

    public async Task<IEnumerable<Repositories.Stores.Entities.Store>> LoadAllStoresAsync(IServiceScope assertionScope)
    {
        await using var storeContext = GetContextInstance<StoreContext>(assertionScope);

        return await storeContext.Stores.AsNoTracking()
            .Include(s => s.Sections)
            .ToListAsync();
    }

    public async Task<IEnumerable<Repositories.ShoppingLists.Entities.ShoppingList>> LoadAllShoppingListsAsync(
        IServiceScope assertionScope)
    {
        await using var shoppingListContext = GetContextInstance<ShoppingListContext>(assertionScope);

        return await shoppingListContext.ShoppingLists.AsNoTracking()
            .Include(l => l.ItemsOnList)
            .Include(l => l.Discounts)
            .ToListAsync();
    }

    public async Task<IEnumerable<RecipeTag>> LoadAllRecipeTagsAsync(IServiceScope assertionScope)
    {
        await using var dbContext = GetContextInstance<RecipeTagContext>(assertionScope);

        return await dbContext.RecipeTags.AsNoTracking().ToListAsync();
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