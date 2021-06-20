using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports
{
    public interface IItemRepository
    {
        Task<IEnumerable<IStoreItem>> FindByAsync(StoreId storeId, CancellationToken cancellationToken);

        Task<IStoreItem> FindByAsync(ItemId storeItemId, CancellationToken cancellationToken);

        Task<IStoreItem> FindByAsync(TemporaryItemId temporaryItemId, CancellationToken cancellationToken);

        Task<IEnumerable<IStoreItem>> FindPermanentByAsync(IEnumerable<StoreId> storeIds,
            IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds,
            CancellationToken cancellationToken);

        Task<IEnumerable<IStoreItem>> FindActiveByAsync(string searchInput, StoreId storeId, CancellationToken cancellationToken);

        Task<IEnumerable<IStoreItem>> FindActiveByAsync(ItemCategoryId itemCategoryId, CancellationToken cancellationToken);

        Task<IStoreItem> StoreAsync(IStoreItem storeItem, CancellationToken cancellationToken);
        Task<IEnumerable<IStoreItem>> FindByAsync(IEnumerable<ItemId> itemIds, CancellationToken cancellationToken);
    }
}