using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion.ItemSearchReadModels;

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

    public async Task<IEnumerable<SearchItemForShoppingResultReadModel>> ConvertAsync(IEnumerable<IItem> items,
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

                IItemAvailability storeAvailability = item.Availabilities
                    .Single(av => av.StoreId == store.Id);

                var section = store.Sections.Single(s => s.Id == storeAvailability.DefaultSectionId);

                return new SearchItemForShoppingResultReadModel(
                    item.Id,
                    null,
                    item.Name.Value,
                    item.ItemQuantity.Type.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                    storeAvailability.Price,
                    manufacturer is null ?
                        null :
                        new ManufacturerReadModel(manufacturer),
                    itemCategory is null ?
                        null :
                        new ItemCategoryReadModel(itemCategory),
                    new SectionReadModel(section));
            });
    }

    public async Task<IEnumerable<SearchItemForShoppingResultReadModel>> ConvertAsync(
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
                IItemAvailability storeAvailability = type.Availabilities
                    .Single(av => av.StoreId == store.Id);

                var section = store.Sections.Single(s => s.Id == storeAvailability.DefaultSectionId);

                return new SearchItemForShoppingResultReadModel(
                    item.Id,
                    type.Id,
                    $"{item.Name} {type.Name}",
                    item.ItemQuantity.Type.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                    storeAvailability.Price,
                    manufacturer is null ?
                        null :
                        new ManufacturerReadModel(manufacturer),
                    itemCategory is null ?
                        null :
                        new ItemCategoryReadModel(itemCategory),
                    new SectionReadModel(section));
            });
        });
    }

    private async Task<Dictionary<ManufacturerId, IManufacturer>> GetManufacturers(IEnumerable<IItem> items,
        CancellationToken cancellationToken)
    {
        var manufacturerIds = items
            .Where(i => i.ManufacturerId != null)
            .Select(i => i.ManufacturerId!.Value)
            .Distinct();
        return (await _manufacturerRepository.FindByAsync(manufacturerIds, cancellationToken))
            .ToDictionary(m => m.Id);
    }

    private async Task<Dictionary<ItemCategoryId, IItemCategory>> GetItemCategories(IEnumerable<IItem> items,
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