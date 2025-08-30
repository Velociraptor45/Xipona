using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Ports;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.Domain.Items.Services.Searches;

public interface IItemMergeSearchService
{
    Task<IEnumerable<SearchItemsForMergeResult>> SearchAsync(ItemCategoryId itemCategoryId,
        ManufacturerId? manufacturerId, ItemQuantity itemQuantity,
        IEnumerable<ItemId> excludedItemIds);
}

public class ItemMergeSearchService : IItemMergeSearchService
{
    private readonly IItemRepository _itemRepository;

    public ItemMergeSearchService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<IEnumerable<SearchItemsForMergeResult>> SearchAsync(ItemCategoryId itemCategoryId,
        ManufacturerId? manufacturerId, ItemQuantity itemQuantity, IEnumerable<ItemId> excludedItemIds)
    {
        var items = await _itemRepository.FindForMergeByAsync(itemCategoryId, manufacturerId, itemQuantity, excludedItemIds);
        return items.Select(i => new SearchItemsForMergeResult(
            i.Id, i.Name, i.ItemCategoryId!.Value, i.ManufacturerId, i.ItemQuantity));
    }
}

public record SearchItemsForMergeResult(ItemId ItemId, ItemName ItemName, ItemCategoryId ItemCategoryId,
    ManufacturerId? ManufacturerId, ItemQuantity ItemQuantity);