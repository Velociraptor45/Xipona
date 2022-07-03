using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;
using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Adapters;
using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Adapters;
using ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Adapters;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Adapters;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Adapters;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Contexts;
using ShoppingList.Api.Vault.Configs;
using System.Data.Common;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, string? connectionString = null)
    {
        var assembly = Assembly.GetExecutingAssembly()!;
        var version = Assembly.GetEntryAssembly()!.GetName().Version!;
        var serverVersion = new MySqlServerVersion(new Version(version.Major, version.Minor, version.Build));

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

        services.AddScoped<IList<DbContext>>(serviceProvider => GetAllDbContextInstances(serviceProvider).ToList());

        services.AddDbContext<ShoppingListContext>(
            (serviceProvider, options) =>
            {
                var connection = serviceProvider.GetService<DbConnection>();
                options.UseMySql(connection, serverVersion);
            });
        services.AddDbContext<ItemCategoryContext>(
            (serviceProvider, options) =>
            {
                var connection = serviceProvider.GetService<DbConnection>();
                options.UseMySql(connection, serverVersion);
            });
        services.AddDbContext<ManufacturerContext>(
            (serviceProvider, options) =>
            {
                var connection = serviceProvider.GetService<DbConnection>();
                options.UseMySql(connection, serverVersion);
            });
        services.AddDbContext<ItemContext>(
            (serviceProvider, options) =>
            {
                var connection = serviceProvider.GetService<DbConnection>();
                options.UseMySql(connection, serverVersion);
            });
        services.AddDbContext<StoreContext>(
            (serviceProvider, options) =>
            {
                var connection = serviceProvider.GetService<DbConnection>();
                options.UseMySql(connection, serverVersion);
            });

        services.AddTransient<IShoppingListRepository, ShoppingListRepository>();
        services.AddTransient<IItemRepository, ItemRepository>();
        services.AddTransient<IItemTypeReadRepository, ItemTypeReadRepository>();
        services.AddTransient<IItemCategoryRepository, ItemCategoryRepository>();
        services.AddTransient<IManufacturerRepository, ManufacturerRepository>();
        services.AddTransient<IStoreRepository, StoreRepository>();
        services.AddScoped<ITransactionGenerator, TransactionGenerator>();

        services.AddImplementationOfGenericType(assembly, typeof(IToEntityConverter<,>));
        services.AddImplementationOfGenericType(assembly, typeof(IToDomainConverter<,>));
    }

#pragma warning disable S1172 // Unused method parameters should be removed

    private static IEnumerable<DbContext> GetAllDbContextInstances(IServiceProvider serviceProvider)
    {
        var types = GetAllDbContextTypes();
        var instances = types.Select(serviceProvider.GetRequiredService);
        foreach (var instance in instances)
        {
            yield return (DbContext)instance;
        }
    }

#pragma warning restore S1172 // Unused method parameters should be removed

    private static IEnumerable<Type> GetAllDbContextTypes()
    {
        // todo make this generic
        yield return typeof(ShoppingListContext);
        yield return typeof(ItemCategoryContext);
        yield return typeof(ManufacturerContext);
        yield return typeof(ItemContext);
        yield return typeof(StoreContext);
    }
}