using ShoppingList.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Ports
{
    public interface IShoppingListRepository
    {
        Task<Models.ShoppingList> FindActiveByStoreIdAsync(StoreId storeId, CancellationToken cancellationToken);
        Task<IEnumerable<Store>> FindActiveStoresAsync();
    }
}