using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Services;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
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
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Items;

public static class ServiceCollectionExtensions
{
    internal static void AddItems(this IServiceCollection services)
    {
        services.AddTransient<IItemFactory, ItemFactory>();
        services.AddTransient<IItemAvailabilityFactory, ItemAvailabilityFactory>();

        services.AddTransient<Func<CancellationToken, IAvailabilityValidationService>>(provider =>
        {
            return ct => new AvailabilityValidationService(
                provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>(),
                ct);
        });

        services.AddTransient<Func<CancellationToken, IItemValidationService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            return cancellationToken => new ItemValidationService(itemRepository, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemSearchReadModelConversionService>>(provider =>
        {
            return ct => new ItemSearchReadModelConversionService(
                provider.GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>(),
                provider.GetRequiredService<Func<CancellationToken, IManufacturerRepository>>(),
                ct);
        });
        services.AddTransient<Func<CancellationToken, IItemReadModelConversionService>>(provider =>
        {
            return ct => new ItemReadModelConversionService(
                provider.GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>(),
                provider.GetRequiredService<Func<CancellationToken, IManufacturerRepository>>(),
                provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>(),
                ct);
        });

        services.AddTransient<Func<CancellationToken, IItemAvailabilityReadModelConversionService>>(provider =>
        {
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            return cancellationToken =>
                new ItemAvailabilityReadModelConversionService(storeRepositoryDelegate, cancellationToken);
        });

        services.AddTransient<IItemTypeFactory, ItemTypeFactory>();

        services.AddTransient<Func<CancellationToken, IItemModificationService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var shoppingListRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IShoppingListRepository>>();
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            return cancellationToken => new ItemModificationService(itemRepository, validatorDelegate,
                shoppingListRepositoryDelegate, storeRepositoryDelegate, cancellationToken);
        });
        services.AddTransient<Func<CancellationToken, IItemUpdateService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var dateTimeService = provider.GetRequiredService<IDateTimeService>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();

            return cancellationToken => new ItemUpdateService(itemRepository, validatorDelegate,
                dateTimeService, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemSearchService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var shoppingListRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IShoppingListRepository>>();
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            var itemTypeReadRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemTypeReadRepository>>();
            var itemCategoryRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>();
            var conversionServiceDelegate = provider
                .GetRequiredService<Func<CancellationToken, IItemSearchReadModelConversionService>>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            var availabilityConverterDelegate = provider.GetRequiredService<
                Func<CancellationToken, IItemAvailabilityReadModelConversionService>>();

            return cancellationToken => new ItemSearchService(itemRepository, shoppingListRepositoryDelegate,
                storeRepositoryDelegate, itemTypeReadRepositoryDelegate, itemCategoryRepositoryDelegate, conversionServiceDelegate,
                validatorDelegate, availabilityConverterDelegate, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemCreationService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            var itemFactory = provider.GetRequiredService<IItemFactory>();
            var conversionServiceDelegate = provider.GetRequiredService<Func<CancellationToken, IItemReadModelConversionService>>();
            return cancellationToken =>
                new ItemCreationService(itemRepository, validatorDelegate, itemFactory, conversionServiceDelegate,
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
            return cancellationToken =>
                new ItemDeletionService(itemRepository, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemQueryService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var conversionServiceDelegate = provider
                .GetRequiredService<Func<CancellationToken, IItemReadModelConversionService>>();
            return cancellationToken =>
                new ItemQueryService(itemRepository, conversionServiceDelegate, cancellationToken);
        });

        services.AddTransient<IQuantitiesQueryService, QuantitiesQueryService>();
    }
}