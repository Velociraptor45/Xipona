using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Items.Ports;

public interface IItemRepository
{
    Task<IEnumerable<IItem>> FindByAsync(IEnumerable<ItemId> itemIds);

    Task<IEnumerable<IItem>> FindPermanentByAsync(IEnumerable<StoreId> storeIds,
        IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds);

    Task<IEnumerable<IItem>> FindActiveByAsync(ManufacturerId manufacturerId);

    Task<IEnumerable<IItem>> FindActiveByAsync(StoreId storeId);

    Task<IEnumerable<IItem>> FindActiveByAsync(string searchInput, int page, int pageSize);

    Task<IEnumerable<IItem>> FindActiveByAsync(ItemCategoryId itemCategoryId);

    Task<IEnumerable<IItem>> FindActiveByAsync(IEnumerable<ItemCategoryId> itemCategoryIds, StoreId storeId,
        IEnumerable<ItemId> excludedItemIds);

    Task<IItem?> FindActiveByAsync(TemporaryItemId temporaryItemId);

    Task<IItem?> FindActiveByAsync(ItemId itemId);

    Task<IItem?> FindActiveByAsync(ItemId itemId, ItemTypeId? itemTypeId);

    Task<IEnumerable<IItem>> FindActiveByAsync(SectionId sectionId);

    Task<IEnumerable<IItem>> FindActiveByAsync(IEnumerable<ItemId> itemIds);

    Task<IEnumerable<IItem>> FindActiveByAsync(string searchInput, StoreId storeId,
        IEnumerable<ItemId> excludedItemIds, int? limit);

    Task<int> GetTotalCountByAsync(string searchInput);

    Task<IItem> StoreAsync(IItem item);

    Task<IEnumerable<IItem>> FindForMergeByAsync(ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId,
        ItemQuantity itemQuantity, IEnumerable<ItemId> excludedItemIds);
}