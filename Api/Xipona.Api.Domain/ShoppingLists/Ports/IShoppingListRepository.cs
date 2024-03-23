using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Ports;

public interface IShoppingListRepository
{
    Task<IShoppingList?> FindActiveByAsync(StoreId storeId);

    Task<IEnumerable<IShoppingList>> FindActiveByAsync(ItemId itemId);

    Task<IEnumerable<IShoppingList>> FindActiveByAsync(IEnumerable<StoreId> storeIds);

    Task<IEnumerable<IShoppingList>> FindByAsync(ItemTypeId typeId);

    Task<IShoppingList?> FindByAsync(ShoppingListId id);

    Task<IEnumerable<IShoppingList>> FindByAsync(ItemId itemId);

    Task StoreAsync(IShoppingList shoppingList);
    Task DeleteAsync(ShoppingListId id);
}