using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Xipona.Api.Core.Services;
using Xipona.Api.Domain.ItemCategories.Ports;
using Xipona.Api.Domain.Items.Models.Factories;
using Xipona.Api.Domain.Items.Ports;
using Xipona.Api.Domain.Items.Services.Conversion;
using Xipona.Api.Domain.Items.Services.Conversion.ItemReadModels;
using Xipona.Api.Domain.Items.Services.Conversion.ItemSearchReadModels;
using Xipona.Api.Domain.Items.Services.Creations;
using Xipona.Api.Domain.Items.Services.Deletions;
using Xipona.Api.Domain.Items.Services.Modifications;
using Xipona.Api.Domain.Items.Services.Queries;
using Xipona.Api.Domain.Items.Services.Queries.Quantities;
using Xipona.Api.Domain.Items.Services.Searches;
using Xipona.Api.Domain.Items.Services.TemporaryItems;
using Xipona.Api.Domain.Items.Services.Updates;
using Xipona.Api.Domain.Items.Services.Validations;
using Xipona.Api.Domain.Manufacturers.Ports;
using Xipona.Api.Domain.Shared.Validations;
using Xipona.Api.Domain.ShoppingLists.Ports;
using Xipona.Api.Domain.Stores.Ports;

namespace Xipona.Api.Domain.Items;

public static class ServiceCollectionExtensions
{
    internal static void AddItems(this IServiceCollection services)
    {
        services.AddTransient<IItemFactory, ItemFactory>();

        services.AddTransient<Func<CancellationToken, IAvailabilityValidationService>>(provider =>
        {
            return ct => new AvailabilityValidationService(
                provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>()(ct));
        });

        services.AddTransient<Func<CancellationToken, IItemValidationService>>(provider =>
        {
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            return ct => new ItemValidationService(itemRepositoryDelegate(ct));
        });

        services.AddTransient<Func<CancellationToken, IItemSearchReadModelConversionService>>(provider =>
        {
            return ct => new ItemSearchReadModelConversionService(
                provider.GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>()(ct),
                provider.GetRequiredService<Func<CancellationToken, IManufacturerRepository>>()(ct),
                provider.GetRequiredService<IMemoryCache>());
        });
        services.AddTransient<Func<CancellationToken, IItemReadModelConversionService>>(provider =>
        {
            return ct => new ItemReadModelConversionService(
                provider.GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>()(ct),
                provider.GetRequiredService<Func<CancellationToken, IManufacturerRepository>>()(ct),
                provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>()(ct),
                provider.GetRequiredService<IMemoryCache>());
        });

        services.AddTransient<Func<CancellationToken, IItemAvailabilityReadModelConversionService>>(provider =>
        {
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            return ct => new ItemAvailabilityReadModelConversionService(storeRepositoryDelegate(ct));
        });

        services.AddTransient<IItemTypeFactory, ItemTypeFactory>();

        services.AddTransient<Func<CancellationToken, IItemModificationService>>(provider =>
        {
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            var shoppingListRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IShoppingListRepository>>();
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            return ct => new ItemModificationService(itemRepositoryDelegate(ct), validatorDelegate(ct),
                shoppingListRepositoryDelegate(ct), storeRepositoryDelegate(ct));
        });
        services.AddTransient<Func<CancellationToken, IItemUpdateService>>(provider =>
        {
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            var dateTimeService = provider.GetRequiredService<IDateTimeService>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();

            return ct => new ItemUpdateService(itemRepositoryDelegate(ct), validatorDelegate(ct),
                dateTimeService);
        });

        services.AddTransient<Func<CancellationToken, IItemSearchService>>(provider =>
        {
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            var manufacturerRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IManufacturerRepository>>();
            var shoppingListRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IShoppingListRepository>>();
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            var itemTypeReadRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemTypeReadRepository>>();
            var itemCategoryRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>();
            var conversionServiceDelegate = provider
                .GetRequiredService<Func<CancellationToken, IItemSearchReadModelConversionService>>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            var availabilityConverterDelegate = provider.GetRequiredService<
                Func<CancellationToken, IItemAvailabilityReadModelConversionService>>();

            return ct => new ItemSearchService(itemRepositoryDelegate(ct), manufacturerRepositoryDelegate(ct),
                shoppingListRepositoryDelegate(ct), storeRepositoryDelegate(ct), itemTypeReadRepositoryDelegate(ct),
                itemCategoryRepositoryDelegate(ct), conversionServiceDelegate(ct), validatorDelegate(ct),
                availabilityConverterDelegate(ct));
        });

        services.AddTransient<Func<CancellationToken, IItemCreationService>>(provider =>
        {
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            var itemFactory = provider.GetRequiredService<IItemFactory>();
            var conversionServiceDelegate = provider.GetRequiredService<Func<CancellationToken, IItemReadModelConversionService>>();
            return ct => new ItemCreationService(itemRepositoryDelegate(ct), validatorDelegate(ct), itemFactory,
                conversionServiceDelegate(ct));
        });

        services.AddTransient<Func<CancellationToken, ITemporaryItemService>>(provider =>
        {
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            var validatorDelegate = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            return ct => new TemporaryItemService(itemRepositoryDelegate(ct), validatorDelegate(ct));
        });

        services.AddTransient<Func<CancellationToken, IItemDeletionService>>(provider =>
        {
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            return ct => new ItemDeletionService(itemRepositoryDelegate(ct));
        });

        services.AddTransient<Func<CancellationToken, IItemQueryService>>(provider =>
        {
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            var conversionServiceDelegate = provider
                .GetRequiredService<Func<CancellationToken, IItemReadModelConversionService>>();
            return ct => new ItemQueryService(itemRepositoryDelegate(ct), conversionServiceDelegate(ct));
        });

        services.AddTransient<Func<CancellationToken, IItemMergeService>>(provider =>
        {
            var itemFactory = provider.GetRequiredService<IItemFactory>();
            var itemTypeFactory = provider.GetRequiredService<IItemTypeFactory>();
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            return ct => new ItemMergeService(itemFactory, itemTypeFactory, itemRepositoryDelegate(ct));
        });

        services.AddTransient<Func<CancellationToken, IItemMergeSearchService>>(provider =>
        {
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            return ct => new ItemMergeSearchService(itemRepositoryDelegate(ct));
        });

        services.AddTransient<Func<CancellationToken, IFavoriteItemService>>(provider =>
        {
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            return ct => new FavoriteItemService(itemRepositoryDelegate(ct));
        });

        services.AddTransient<IQuantitiesQueryService, QuantitiesQueryService>();
    }
}