using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Exchanges;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists;

public static class ServiceCollectionExtensions
{
    internal static void AddShoppingLists(this IServiceCollection services)
    {
        services.AddTransient<IShoppingListItemFactory, ShoppingListItemFactory>();
        services.AddTransient<IShoppingListFactory, ShoppingListFactory>();
        services.AddTransient<IShoppingListSectionFactory, ShoppingListSectionFactory>();

        services.AddTransient<Func<CancellationToken, IShoppingListExchangeService>>(provider =>
        {
            return ct => new ShoppingListExchangeService(
                provider.GetRequiredService<Func<CancellationToken, IShoppingListRepository>>(),
                provider.GetRequiredService<Func<CancellationToken, IAddItemToShoppingListService>>(),
                provider.GetRequiredService<ILogger<ShoppingListExchangeService>>(),
                ct);
        });

        services.AddTransient<Func<CancellationToken, IAddItemToShoppingListService>>(provider =>
        {
            var shoppingListSectionFactory = provider.GetRequiredService<IShoppingListSectionFactory>();
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            var itemFactory = provider.GetRequiredService<IShoppingListItemFactory>();
            var shoppingListRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IShoppingListRepository>>();
            return token => new AddItemToShoppingListService(shoppingListSectionFactory, storeRepositoryDelegate,
                itemRepositoryDelegate, itemFactory, shoppingListRepositoryDelegate, token);
        });

        services.AddTransient<Func<CancellationToken, IShoppingListReadModelConversionService>>(provider =>
        {
            return ct => new ShoppingListReadModelConversionService(
                provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>(),
                provider.GetRequiredService<Func<CancellationToken, IItemRepository>>(),
                provider.GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>(),
                provider.GetRequiredService<Func<CancellationToken, IManufacturerRepository>>(),
                ct
            );
        });

        services.AddTransient<Func<CancellationToken, IShoppingListModificationService>>(provider =>
        {
            var addItemToShoppingListServiceDelegate = provider
                .GetRequiredService<Func<CancellationToken, IAddItemToShoppingListService>>();
            var shoppingListRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IShoppingListRepository>>();
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            var shoppingListSectionFactory = provider.GetRequiredService<IShoppingListSectionFactory>();
            var itemFactory = provider.GetRequiredService<IItemFactory>();
            return cancellationToken =>
                new ShoppingListModificationService(addItemToShoppingListServiceDelegate, shoppingListRepositoryDelegate,
                    itemRepositoryDelegate, storeRepositoryDelegate, shoppingListSectionFactory, itemFactory,
                    cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IShoppingListQueryService>>(provider =>
        {
            var shoppingListRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IShoppingListRepository>>();
            var conversionServiceDelegate = provider
                .GetRequiredService<Func<CancellationToken, IShoppingListReadModelConversionService>>();
            return cancellationToken => new ShoppingListQueryService(shoppingListRepositoryDelegate,
                conversionServiceDelegate, cancellationToken);
        });
    }
}