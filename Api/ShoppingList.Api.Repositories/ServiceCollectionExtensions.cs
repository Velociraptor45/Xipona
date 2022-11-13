using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Repositories.Common.Transactions;
using ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Adapters;
using ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Adapters;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Adapters;
using ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Adapters;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Adapters;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Adapters;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Contexts;
using ProjectHermes.ShoppingList.Api.Vault.Configs;
using System.Data.Common;
using System.Reflection;
using Recipe = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.Recipe;

namespace ProjectHermes.ShoppingList.Api.Repositories;

public static class ServiceCollectionExtensions
{
    private static readonly MySqlServerVersion _sqlServerVersion = new(new Version(10, 3, 27));

    public static void AddRepositories(this IServiceCollection services, string? connectionString = null)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddScoped<DbConnection>(provider =>
        {
            if (connectionString is null)
            {
                var cs = provider.GetRequiredService<ConnectionStrings>();
                connectionString = cs.ShoppingDatabase;
            }
            var connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        });

        services.AddScoped<IList<DbContext>>(
            serviceProvider => GetAllDbContextInstances(serviceProvider, assembly).ToList());

        services.AddDbContext<ShoppingListContext>(SetDbConnection);
        services.AddDbContext<ItemCategoryContext>(SetDbConnection);
        services.AddDbContext<ManufacturerContext>(SetDbConnection);
        services.AddDbContext<ItemContext>(SetDbConnection);
        services.AddDbContext<StoreContext>(SetDbConnection);
        services.AddDbContext<RecipeContext>(SetDbConnection);

        services.AddTransient<IShoppingListRepository, ShoppingListRepository>();
        services.AddTransient<IItemRepository, ItemRepository>();
        services.AddTransient<IItemTypeReadRepository, ItemTypeReadRepository>();
        services.AddTransient<IItemCategoryRepository, ItemCategoryRepository>();
        services.AddTransient<IManufacturerRepository, ManufacturerRepository>();
        services.AddTransient<IStoreRepository, StoreRepository>();
        services.AddTransient<Func<CancellationToken, IRecipeRepository>>(provider =>
        {
            var context = provider.GetRequiredService<RecipeContext>();
            var searchResultToDomainConverter = provider.GetRequiredService<IToDomainConverter<Recipe, RecipeSearchResult>>();
            var toDomainConverter = provider.GetRequiredService<IToDomainConverter<Recipe, IRecipe>>();
            var toContractConverter = provider.GetRequiredService<IToContractConverter<IRecipe, Recipe>>();
            return cancellationToken =>
                new RecipeRepository(context, searchResultToDomainConverter, toDomainConverter, toContractConverter,
                    cancellationToken);
        });
        services.AddScoped(_ => new SemaphoreSlim(1, 1));
        services.AddScoped<ITransactionGenerator, TransactionGenerator>();

        services.AddImplementationOfGenericType(assembly, typeof(IToEntityConverter<,>));
        services.AddImplementationOfGenericType(assembly, typeof(IToContractConverter<,>));
        services.AddImplementationOfGenericType(assembly, typeof(IToDomainConverter<,>));
    }

    private static void SetDbConnection(IServiceProvider serviceProvider, DbContextOptionsBuilder options)
    {
        var connection = serviceProvider.GetService<DbConnection>()!;
        options.UseMySql(connection, _sqlServerVersion);
    }

#pragma warning disable S1172 // Unused method parameters should be removed

    private static IEnumerable<DbContext> GetAllDbContextInstances(IServiceProvider serviceProvider, Assembly assembly)
    {
        var types = GetAllDbContextTypes(assembly);
        var instances = types.Select(serviceProvider.GetRequiredService);
        foreach (var instance in instances)
        {
            yield return (DbContext)instance;
        }
    }

#pragma warning restore S1172 // Unused method parameters should be removed

    private static IEnumerable<Type> GetAllDbContextTypes(Assembly assembly)
    {
        var baseType = typeof(DbContext);
        return assembly.GetTypes().Where(t => t.BaseType == baseType).ToList();
    }
}