using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ProjectHermes.ShoppingList.Api.ApplicationServices;
using ProjectHermes.ShoppingList.Api.Core;
using ProjectHermes.ShoppingList.Api.Domain;
using ProjectHermes.ShoppingList.Api.Repositories;
using ProjectHermes.ShoppingList.Api.Repositories.Common.Transactions;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Contexts;
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
        services.AddRepositories(DockerFixture.ConnectionString);
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

    public async Task<IEnumerable<Recipe>> LoadAllRecipesAsync(IServiceScope assertionScope)
    {
        await using var recipeContext = GetContextInstance<RecipeContext>(assertionScope);

        return await recipeContext.Recipes.AsNoTracking()
            .Include(r => r.Ingredients)
            .Include(r => r.PreparationSteps)
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
            .ToArrayAsync();
    }

    public async Task<IEnumerable<Repositories.Stores.Entities.Store>> LoadAllStoresAsync(IServiceScope assertionScope)
    {
        await using var storeContext = GetContextInstance<StoreContext>(assertionScope);

        return await storeContext.Stores.AsNoTracking()
            .Include(s => s.Sections)
            .ToArrayAsync();
    }

    public async Task<IEnumerable<Repositories.ShoppingLists.Entities.ShoppingList>> LoadAllShoppingListsAsync(
        IServiceScope assertionScope)
    {
        await using var shoppingListContext = GetContextInstance<ShoppingListContext>(assertionScope);

        return await shoppingListContext.ShoppingLists.AsNoTracking()
            .Include(l => l.ItemsOnList)
            .ToArrayAsync();
    }

    public async Task<IEnumerable<RecipeTag>> LoadAllRecipeTagsAsync(IServiceScope assertionScope)
    {
        await using var dbContext = GetContextInstance<RecipeTagContext>(assertionScope);

        return await dbContext.RecipeTags.ToListAsync();
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