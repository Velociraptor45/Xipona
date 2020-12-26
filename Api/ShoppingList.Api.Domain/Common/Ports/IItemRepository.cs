using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Ports
{
    public interface IItemRepository
    {
        Task<IStoreItem> FindByAsync(StoreItemId storeItemId, StoreId storeId, CancellationToken cancellationToken);

        Task<IEnumerable<IStoreItem>> FindByAsync(StoreId storeId, CancellationToken cancellationToken);

        Task<IStoreItem> FindByAsync(StoreItemId storeItemId, CancellationToken cancellationToken);

        Task<IEnumerable<IStoreItem>> FindPermanentByAsync(IEnumerable<StoreId> storeIds,
            IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds,
            CancellationToken cancellationToken);

        Task<IEnumerable<IStoreItem>> FindActiveByAsync(string searchInput, StoreId storeId, CancellationToken cancellationToken);

        Task<IStoreItem> StoreAsync(IStoreItem storeItem, CancellationToken cancellationToken);
    }
}