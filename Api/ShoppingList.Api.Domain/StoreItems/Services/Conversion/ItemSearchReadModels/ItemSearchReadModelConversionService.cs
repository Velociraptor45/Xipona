using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    private readonly IItemCategoryRepository itemCategoryRepository;
    private readonly IManufacturerRepository manufacturerRepository;

    public ItemSearchReadModelConversionService(IItemCategoryRepository itemCategoryRepository,
        IManufacturerRepository manufacturerRepository)
    {
        this.itemCategoryRepository = itemCategoryRepository;
        this.manufacturerRepository = manufacturerRepository;
    }

    public async Task<IEnumerable<ItemSearchReadModel>> ConvertAsync(IEnumerable<IStoreItem> items,
        IStore store, CancellationToken cancellationToken)
    {
        var itemsList = items.ToList();

        var itemCategoryDict = await GetItemCategories(itemsList, cancellationToken);
        var manufaturerDict = await GetManufacturers(itemsList, cancellationToken);

        return itemsList
            .Select(item =>
            {
                IManufacturer? manufacturer =
                    item.ManufacturerId == null ? null : manufaturerDict[item.ManufacturerId.Value];
                IItemCategory? itemCategory =
                    item.ItemCategoryId == null ? null : itemCategoryDict[item.ItemCategoryId.Value];

                IStoreItemAvailability storeAvailability = item.Availabilities
                    .Single(av => av.StoreId == store.Id);

                var section = store.Sections.Single(s => s.Id == storeAvailability.DefaultSectionId);

                return new ItemSearchReadModel(
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

    public async Task<IEnumerable<ItemSearchReadModel>> ConvertAsync(
        IEnumerable<ItemWithMatchingItemTypeIds> itemTypes, IStore store,
        CancellationToken cancellationToken)
    {
        var itemTypesList = itemTypes.ToList();
        var itemsList = itemTypesList.Select(t => t.Item);

        var itemCategoryDict = await GetItemCategories(itemsList, cancellationToken);
        var manufaturerDict = await GetManufacturers(itemsList, cancellationToken);

        return itemTypesList.SelectMany(tuple =>
        {
            var item = tuple.Item;

            IManufacturer? manufacturer =
                item.ManufacturerId == null ? null : manufaturerDict[item.ManufacturerId.Value];
            IItemCategory? itemCategory =
                item.ItemCategoryId == null ? null : itemCategoryDict[item.ItemCategoryId.Value];

            var itemTypeIdsSet = tuple.MatchingItemTypeIds.ToHashSet();
            var requiredItemTypes = item.ItemTypes.Where(t => itemTypeIdsSet.Contains(t.Id));
            return requiredItemTypes.Select(type =>
            {
                IStoreItemAvailability storeAvailability = type.Availabilities
                    .Single(av => av.StoreId == store.Id);

                var section = store.Sections.Single(s => s.Id == storeAvailability.DefaultSectionId);

                return new ItemSearchReadModel(
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
        return (await manufacturerRepository.FindByAsync(manufacturerIds, cancellationToken))
            .ToDictionary(m => m.Id);
    }

    private async Task<Dictionary<ItemCategoryId, IItemCategory>> GetItemCategories(IEnumerable<IStoreItem> items,
        CancellationToken cancellationToken)
    {
        var itemCategoryIds = items
            .Where(i => i.ItemCategoryId != null)
            .Select(i => i.ItemCategoryId!.Value)
            .Distinct();
        return (await itemCategoryRepository.FindByAsync(itemCategoryIds, cancellationToken))
            .ToDictionary(i => i.Id);
    }
}