using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Ports;

public interface IItemRepository
{
    Task<IEnumerable<IItem>> FindByAsync(StoreId storeId, CancellationToken cancellationToken);

    Task<IItem?> FindByAsync(ItemId itemId, CancellationToken cancellationToken);

    Task<IItem?> FindByAsync(TemporaryItemId temporaryItemId, CancellationToken cancellationToken);

    Task<IEnumerable<IItem>> FindByAsync(IEnumerable<ItemId> itemIds, CancellationToken cancellationToken);

    Task<IEnumerable<IItem>> FindByAsync(ManufacturerId manufacturerId, CancellationToken cancellationToken);

    Task<IEnumerable<IItem>> FindPermanentByAsync(IEnumerable<StoreId> storeIds,
        IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds,
        CancellationToken cancellationToken);

    Task<IEnumerable<IItem>> FindActiveByAsync(string searchInput, StoreId storeId, CancellationToken cancellationToken);

    Task<IEnumerable<IItem>> FindActiveByAsync(string searchInput, CancellationToken cancellationToken);

    Task<IEnumerable<IItem>> FindActiveByAsync(ItemCategoryId itemCategoryId, CancellationToken cancellationToken);

    Task<IItem?> FindActiveByAsync(ItemId itemId, ItemTypeId? itemTypeId, CancellationToken cancellationToken);

    Task<IEnumerable<IItem>> FindActiveByAsync(SectionId sectionId, CancellationToken cancellationToken);

    Task<IEnumerable<IItem>> FindActiveByAsync(IEnumerable<ItemId> itemIds, CancellationToken cancellationToken);

    Task<IItem> StoreAsync(IItem item, CancellationToken cancellationToken);
}