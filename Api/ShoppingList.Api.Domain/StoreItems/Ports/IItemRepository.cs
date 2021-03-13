using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports
{
    public interface IItemRepository
    {
        Task<IEnumerable<IStoreItem>> FindByAsync(ShoppingListStoreId storeId, CancellationToken cancellationToken);

        Task<IStoreItem> FindByAsync(StoreItemId storeItemId, CancellationToken cancellationToken);

        Task<IEnumerable<IStoreItem>> FindPermanentByAsync(IEnumerable<ShoppingListStoreId> storeIds,
            IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds,
            CancellationToken cancellationToken);

        Task<IEnumerable<IStoreItem>> FindActiveByAsync(string searchInput, ShoppingListStoreId storeId, CancellationToken cancellationToken);

        Task<IEnumerable<IStoreItem>> FindActiveByAsync(ItemCategoryId itemCategoryId, CancellationToken cancellationToken);

        Task<IStoreItem> StoreAsync(IStoreItem storeItem, CancellationToken cancellationToken);
    }
}