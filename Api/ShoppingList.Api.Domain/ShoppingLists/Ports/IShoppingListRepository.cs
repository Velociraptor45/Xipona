using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports
{
    public interface IShoppingListRepository
    {
        Task<IShoppingList?> FindActiveByAsync(StoreId storeId, CancellationToken cancellationToken);

        Task<IEnumerable<IShoppingList>> FindActiveByAsync(ItemId storeItemId, CancellationToken cancellationToken);
        Task<IEnumerable<IShoppingList>> FindActiveByAsync(ItemTypeId typeId, CancellationToken cancellationToken);
        Task<IShoppingList?> FindByAsync(ShoppingListId id, CancellationToken cancellationToken);

        Task<IEnumerable<IShoppingList>> FindByAsync(ItemId storeItemId, CancellationToken cancellationToken);

        Task StoreAsync(IShoppingList shoppingList, CancellationToken cancellationToken);
    }
}