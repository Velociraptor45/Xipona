using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.StoreItemReadModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemCreations;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemDeletions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModifications;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemQueries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemUpdates;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Search;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreCreations;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Domain;

public static class ServiceCollectionExtensions
{
    public static void AddDomain(this IServiceCollection services)
    {
        var assembly = typeof(ServiceCollectionExtensions).Assembly;
        services.AddHandlersForAssembly(assembly);
        services.AddImplementationOfGenericType(assembly, typeof(IToReadModelConverter<,>));

        services.AddTransient<IItemCategoryFactory, ItemCategoryFactory>();
        services.AddTransient<IManufacturerFactory, ManufacturerFactory>();

        services.AddTransient<IStoreItemFactory, StoreItemFactory>();
        services.AddTransient<IStoreItemAvailabilityFactory, StoreItemAvailabilityFactory>();
        services.AddTransient<IStoreFactory, StoreFactory>();
        services.AddTransient<IShoppingListItemFactory, ShoppingListItemFactory>();
        services.AddTransient<IShoppingListFactory, ShoppingListFactory>();
        services.AddTransient<IStoreSectionFactory, StoreSectionFactory>();
        services.AddTransient<IShoppingListSectionFactory, ShoppingListSectionFactory>();

        services.AddTransient<IAvailabilityValidationService, AvailabilityValidationService>();
        services.AddTransient<IManufacturerValidationService, ManufacturerValidationService>();
        services.AddTransient<IItemCategoryValidationService, ItemCategoryValidationService>();

        services.AddTransient<IShoppingListUpdateService, ShoppingListUpdateService>();
        services.AddTransient<IAddItemToShoppingListService, AddItemToShoppingListService>();

        services.AddTransient<IShoppingListReadModelConversionService, ShoppingListReadModelConversionService>();
        services.AddTransient<IItemSearchReadModelConversionService, ItemSearchReadModelConversionService>();
        services.AddTransient<IStoreItemReadModelConversionService, StoreItemReadModelConversionService>();

        services.AddTransient<IItemTypeFactory, ItemTypeFactory>();

        // services
        services.AddTransient<Func<CancellationToken, IItemUpdateService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var itemTypeFactory = provider.GetRequiredService<IItemTypeFactory>();
            var storeItemFactory = provider.GetRequiredService<IStoreItemFactory>();
            var shoppingListUpdateService = provider.GetRequiredService<IShoppingListUpdateService>();
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
            return cancellationToken =>
                new ItemCreationService(itemRepository, validatorDelegate, itemFactory, cancellationToken);
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

        services.AddTransient<Func<CancellationToken, IStoreCreationService>>(provider =>
        {
            var storeRepository = provider.GetRequiredService<IStoreRepository>();
            var storeFactory = provider.GetRequiredService<IStoreFactory>();
            var shoppingListFactory = provider.GetRequiredService<IShoppingListFactory>();
            var shoppingListRepository = provider.GetRequiredService<IShoppingListRepository>();
            return cancellationToken =>
                new StoreCreationService(storeRepository, storeFactory, shoppingListFactory, shoppingListRepository,
                    cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemModificationService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var shoppingListRepository = provider.GetRequiredService<IShoppingListRepository>();
            var validatorDelegat = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            return cancellationToken => new ItemModificationService(itemRepository, validatorDelegat,
                shoppingListRepository, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IValidator>>(provider =>
        {
            var availabilityValidationService = provider.GetRequiredService<IAvailabilityValidationService>();
            var itemCategoryValidationService = provider.GetRequiredService<IItemCategoryValidationService>();
            var manufacturerValidationService = provider.GetRequiredService<IManufacturerValidationService>();
            return cancellationToken => new Validator(availabilityValidationService,
                itemCategoryValidationService, manufacturerValidationService, cancellationToken);
        });
    }

    private static void AddHandlersForAssembly(this IServiceCollection services, Assembly assembly)
    {
        services.AddQueryHandlersForAssembly(assembly);
        services.AddCommandHandlersForAssembly(assembly);
    }

    private static void AddQueryHandlersForAssembly(this IServiceCollection services, Assembly assembly)
    {
        var handlerType = typeof(IQueryHandler<,>);
        services.AddImplementationOfGenericType(assembly, handlerType);
    }

    private static void AddCommandHandlersForAssembly(this IServiceCollection services, Assembly assembly)
    {
        var handlerType = typeof(ICommandHandler<,>);
        services.AddImplementationOfGenericType(assembly, handlerType);
    }
}