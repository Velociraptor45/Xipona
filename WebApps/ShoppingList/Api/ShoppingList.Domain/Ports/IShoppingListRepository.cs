using ShoppingList.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Ports
{
    public interface IShoppingListRepository
    {
        Task<Models.ShoppingList> FindActiveByAsync(StoreId storeId, CancellationToken cancellationToken);

        Task<IEnumerable<Store>> FindActiveStoresAsync(CancellationToken cancellationToken);

        Task<Models.ShoppingList> FindByAsync(ShoppingListId id, CancellationToken cancellationToken);

        Task<IEnumerable<ItemCategory>> FindItemCategoriesByAsync(string searchInput,
            CancellationToken cancellationToken);

        Task<ItemCategory> FindItemCategoryByAsync(ItemCategoryId id, CancellationToken cancellationToken);

        Task<Manufacturer> FindManufacturerByAsync(ManufacturerId id, CancellationToken cancellationToken);

        Task<IEnumerable<Manufacturer>> FindManufacturersByAsync(string searchInput, CancellationToken cancellationToken);

        Task StoreAsync(Models.ShoppingList shoppingList, CancellationToken cancellationToken);
    }
}