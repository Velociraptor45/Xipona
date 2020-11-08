using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Ports
{
    public interface IItemRepository
    {
        Task<StoreItem> FindByAsync(StoreItemId storeItemId, StoreId storeId, CancellationToken cancellationToken);

        Task<IEnumerable<StoreItem>> FindByAsync(string searchInput, StoreId storeId, CancellationToken cancellationToken);

        Task<IEnumerable<StoreItem>> FindByAsync(StoreId storeId, CancellationToken cancellationToken);

        Task<IEnumerable<StoreItem>> FindByAsync(IEnumerable<StoreId> storeIds,
            IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds,
            CancellationToken cancellationToken);

        Task<StoreItem> FindByAsync(StoreItemId storeItemId, CancellationToken cancellationToken);

        Task<bool> IsValidIdAsync(StoreItemId id, CancellationToken cancellationToken);

        Task<StoreItem> StoreAsync(StoreItem storeItem, CancellationToken cancellationToken);
    }
}