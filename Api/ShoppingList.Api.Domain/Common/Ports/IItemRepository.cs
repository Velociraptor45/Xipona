using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Ports
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

        Task<StoreItem> StoreAsync(StoreItem storeItem, CancellationToken cancellationToken);
    }
}