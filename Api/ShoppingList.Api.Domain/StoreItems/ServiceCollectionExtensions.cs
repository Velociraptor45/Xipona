using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Exchanges;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.StoreItemReadModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Deletions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Searches;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Updates;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems;

public static class ServiceCollectionExtensions
{
    internal static void AddItems(this IServiceCollection services)
    {
        services.AddTransient<IStoreItemFactory, StoreItemFactory>();
        services.AddTransient<IStoreItemAvailabilityFactory, StoreItemAvailabilityFactory>();

        services.AddTransient<IAvailabilityValidationService, AvailabilityValidationService>();
        services.AddTransient<IItemSearchReadModelConversionService, ItemSearchReadModelConversionService>();
        services.AddTransient<IStoreItemReadModelConversionService, StoreItemReadModelConversionService>();

        services.AddTransient<IItemTypeFactory, ItemTypeFactory>();

        services.AddTransient<Func<CancellationToken, IItemModificationService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var shoppingListRepository = provider.GetRequiredService<IShoppingListRepository>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            return cancellationToken => new ItemModificationService(itemRepository, validatorDelegate,
                shoppingListRepository, cancellationToken);
        });
        services.AddTransient<Func<CancellationToken, IItemUpdateService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var itemTypeFactory = provider.GetRequiredService<IItemTypeFactory>();
            var storeItemFactory = provider.GetRequiredService<IStoreItemFactory>();
            var shoppingListUpdateService = provider.GetRequiredService<IShoppingListExchangeService>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();

            return cancellationToken => new ItemUpdateService(itemRepository, validatorDelegate, itemTypeFactory,
                storeItemFactory, shoppingListUpdateService, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemSearchService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var shoppingListRepository = provider.GetRequiredService<IShoppingListRepository>();
            var storeRepository = provider.GetRequiredService<IStoreRepository>();
            var itemTypeReadRepository = provider.GetRequiredService<IItemTypeReadRepository>();
            var conversionService = provider.GetRequiredService<IItemSearchReadModelConversionService>();
            return cancellationToken => new ItemSearchService(itemRepository, shoppingListRepository,
                storeRepository, itemTypeReadRepository, conversionService, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemCreationService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            var itemFactory = provider.GetRequiredService<IStoreItemFactory>();
            var conversionService = provider.GetRequiredService<IStoreItemReadModelConversionService>();
            return cancellationToken =>
                new ItemCreationService(itemRepository, validatorDelegate, itemFactory, conversionService,
                    cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, ITemporaryItemService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            return cancellationToken =>
                new TemporaryItemService(itemRepository, validatorDelegate, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemDeletionService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var shoppingListRepository = provider.GetRequiredService<IShoppingListRepository>();
            return cancellationToken =>
                new ItemDeletionService(itemRepository, shoppingListRepository, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemQueryService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var conversionService = provider.GetRequiredService<IStoreItemReadModelConversionService>();
            return cancellationToken =>
                new ItemQueryService(itemRepository, conversionService, cancellationToken);
        });

        services.AddTransient<IQuantitiesQueryService, QuantitiesQueryService>();
    }
}