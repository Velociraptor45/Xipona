using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;

public interface IShoppingListUpdateService
{
    Task ExchangeItemAsync(ItemId oldItemId, IStoreItem newItem, CancellationToken cancellationToken);
}