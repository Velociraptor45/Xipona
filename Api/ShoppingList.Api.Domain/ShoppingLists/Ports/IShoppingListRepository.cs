using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;

public interface IShoppingListRepository
{
    Task<IShoppingList?> FindActiveByAsync(StoreId storeId, CancellationToken cancellationToken);

    Task<IEnumerable<IShoppingList>> FindActiveByAsync(ItemId itemId, CancellationToken cancellationToken);

    Task<IEnumerable<IShoppingList>> FindActiveByAsync(IEnumerable<StoreId> storeIds,
        CancellationToken cancellationToken);

    Task<IEnumerable<IShoppingList>> FindByAsync(ItemTypeId typeId, CancellationToken cancellationToken);

    Task<IShoppingList?> FindByAsync(ShoppingListId id, CancellationToken cancellationToken);

    Task<IEnumerable<IShoppingList>> FindByAsync(ItemId itemId, CancellationToken cancellationToken);

    Task StoreAsync(IShoppingList shoppingList, CancellationToken cancellationToken);
}