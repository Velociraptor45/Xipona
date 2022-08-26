using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion.ItemReadModels;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Deletions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries.Quantities;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Exchanges;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Items;

public static class ServiceCollectionExtensions
{
    internal static void AddItems(this IServiceCollection services)
    {
        services.AddTransient<IItemFactory, ItemFactory>();
        services.AddTransient<IItemAvailabilityFactory, ItemAvailabilityFactory>();

        services.AddTransient<IAvailabilityValidationService, AvailabilityValidationService>();
        services.AddTransient<IItemSearchReadModelConversionService, ItemSearchReadModelConversionService>();
        services.AddTransient<IItemReadModelConversionService, ItemReadModelConversionService>();

        services.AddTransient<Func<CancellationToken, IItemAvailabilityReadModelConversionService>>(provider =>
        {
            var storeRepository = provider.GetRequiredService<IStoreRepository>();
            return cancellationToken =>
                new ItemAvailabilityReadModelConversionService(storeRepository, cancellationToken);
        });

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
            var itemFactory = provider.GetRequiredService<IItemFactory>();
            var shoppingListUpdateService = provider.GetRequiredService<IShoppingListExchangeService>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();

            return cancellationToken => new ItemUpdateService(itemRepository, validatorDelegate, itemTypeFactory,
                itemFactory, shoppingListUpdateService, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemSearchService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var shoppingListRepository = provider.GetRequiredService<IShoppingListRepository>();
            var storeRepository = provider.GetRequiredService<IStoreRepository>();
            var itemTypeReadRepository = provider.GetRequiredService<IItemTypeReadRepository>();
            var conversionService = provider.GetRequiredService<IItemSearchReadModelConversionService>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            var availabilityConverterDelegate = provider.GetRequiredService<
                Func<CancellationToken, IItemAvailabilityReadModelConversionService>>();

            return cancellationToken => new ItemSearchService(itemRepository, shoppingListRepository,
                storeRepository, itemTypeReadRepository, conversionService, validatorDelegate,
                availabilityConverterDelegate, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemCreationService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            var itemFactory = provider.GetRequiredService<IItemFactory>();
            var conversionService = provider.GetRequiredService<IItemReadModelConversionService>();
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
            var conversionService = provider.GetRequiredService<IItemReadModelConversionService>();
            return cancellationToken =>
                new ItemQueryService(itemRepository, conversionService, cancellationToken);
        });

        services.AddTransient<IQuantitiesQueryService, QuantitiesQueryService>();
    }
}