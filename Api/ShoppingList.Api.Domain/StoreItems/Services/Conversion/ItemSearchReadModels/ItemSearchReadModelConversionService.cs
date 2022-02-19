using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.ItemSearchReadModels;

public class ItemSearchReadModelConversionService : IItemSearchReadModelConversionService
{
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IManufacturerRepository _manufacturerRepository;

    public ItemSearchReadModelConversionService(IItemCategoryRepository itemCategoryRepository,
        IManufacturerRepository manufacturerRepository)
    {
        _itemCategoryRepository = itemCategoryRepository;
        _manufacturerRepository = manufacturerRepository;
    }

    public async Task<IEnumerable<ItemForShoppingListSearchReadModel>> ConvertAsync(IEnumerable<IStoreItem> items,
        IStore store, CancellationToken cancellationToken)
    {
        var itemsList = items.ToList();

        var itemCategoryDict = await GetItemCategories(itemsList, cancellationToken);
        var manufacturerDict = await GetManufacturers(itemsList, cancellationToken);

        return itemsList
            .Select(item =>
            {
                IManufacturer? manufacturer =
                    item.ManufacturerId == null ? null : manufacturerDict[item.ManufacturerId.Value];
                IItemCategory? itemCategory =
                    item.ItemCategoryId == null ? null : itemCategoryDict[item.ItemCategoryId.Value];

                IStoreItemAvailability storeAvailability = item.Availabilities
                    .Single(av => av.StoreId == store.Id);

                var section = store.Sections.Single(s => s.Id == storeAvailability.DefaultSectionId);

                return new ItemForShoppingListSearchReadModel(
                    item.Id,
                    null,
                    item.Name,
                    item.QuantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                    storeAvailability.Price,
                    manufacturer?.ToReadModel(),
                    itemCategory?.ToReadModel(),
                    section.ToReadModel());
            });
    }

    public async Task<IEnumerable<ItemForShoppingListSearchReadModel>> ConvertAsync(
        IEnumerable<ItemWithMatchingItemTypeIds> itemTypes, IStore store,
        CancellationToken cancellationToken)
    {
        var itemTypesList = itemTypes.ToList();
        var itemsList = itemTypesList.Select(t => t.Item).ToList();

        var itemCategoryDict = await GetItemCategories(itemsList, cancellationToken);
        var manufacturerDict = await GetManufacturers(itemsList, cancellationToken);

        return itemTypesList.SelectMany(tuple =>
        {
            var item = tuple.Item;

            IManufacturer? manufacturer =
                item.ManufacturerId == null ? null : manufacturerDict[item.ManufacturerId.Value];
            IItemCategory? itemCategory =
                item.ItemCategoryId == null ? null : itemCategoryDict[item.ItemCategoryId.Value];

            var itemTypeIdsSet = tuple.MatchingItemTypeIds.ToHashSet();
            var requiredItemTypes = item.ItemTypes.Where(t => itemTypeIdsSet.Contains(t.Id));
            return requiredItemTypes.Select(type =>
            {
                IStoreItemAvailability storeAvailability = type.Availabilities
                    .Single(av => av.StoreId == store.Id);

                var section = store.Sections.Single(s => s.Id == storeAvailability.DefaultSectionId);

                return new ItemForShoppingListSearchReadModel(
                    item.Id,
                    type.Id,
                    $"{item.Name} {type.Name}",
                    item.QuantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                    storeAvailability.Price,
                    manufacturer?.ToReadModel(),
                    itemCategory?.ToReadModel(),
                    section.ToReadModel());
            });
        });
    }

    private async Task<Dictionary<ManufacturerId, IManufacturer>> GetManufacturers(IEnumerable<IStoreItem> items,
        CancellationToken cancellationToken)
    {
        var manufacturerIds = items
            .Where(i => i.ManufacturerId != null)
            .Select(i => i.ManufacturerId!.Value)
            .Distinct();
        return (await _manufacturerRepository.FindByAsync(manufacturerIds, cancellationToken))
            .ToDictionary(m => m.Id);
    }

    private async Task<Dictionary<ItemCategoryId, IItemCategory>> GetItemCategories(IEnumerable<IStoreItem> items,
        CancellationToken cancellationToken)
    {
        var itemCategoryIds = items
            .Where(i => i.ItemCategoryId != null)
            .Select(i => i.ItemCategoryId!.Value)
            .Distinct();
        return (await _itemCategoryRepository.FindByAsync(itemCategoryIds, cancellationToken))
            .ToDictionary(i => i.Id);
    }
}