using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
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

        services.AddTransient<IShoppingListExchangeService, ShoppingListExchangeService>();
        services.AddTransient<Func<CancellationToken, IAddItemToShoppingListService>>(provider =>
        {
            var shoppingListSectionFactory = provider.GetRequiredService<IShoppingListSectionFactory>();
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var itemFactory = provider.GetRequiredService<IShoppingListItemFactory>();
            var shoppingListRepository = provider.GetRequiredService<IShoppingListRepository>();
            return token => new AddItemToShoppingListService(shoppingListSectionFactory, storeRepositoryDelegate,
                itemRepository, itemFactory, shoppingListRepository, token);
        });

        services.AddTransient<Func<CancellationToken, IShoppingListReadModelConversionService>>(provider =>
        {
            return ct => new ShoppingListReadModelConversionService(
                provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>(),
                provider.GetRequiredService<IItemRepository>(),
                provider.GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>(),
                provider.GetRequiredService<Func<CancellationToken, IManufacturerRepository>>(),
                ct
            );
        });

        services.AddTransient<Func<CancellationToken, IShoppingListModificationService>>(provider =>
        {
            var shoppingListRepository = provider.GetRequiredService<IShoppingListRepository>();
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            var shoppingListSectionFactory = provider.GetRequiredService<IShoppingListSectionFactory>();
            return cancellationToken =>
                new ShoppingListModificationService(shoppingListRepository, itemRepository, storeRepositoryDelegate,
                    shoppingListSectionFactory, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IShoppingListQueryService>>(provider =>
        {
            var shoppingListRepository = provider.GetRequiredService<IShoppingListRepository>();
            var conversionServiceDelegate = provider
                .GetRequiredService<Func<CancellationToken, IShoppingListReadModelConversionService>>();
            return cancellationToken =>
                new ShoppingListQueryService(shoppingListRepository, conversionServiceDelegate, cancellationToken);
        });
    }
}