using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.ShoppingListExchanges;

public interface IShoppingListExchangeService
{
    Task ExchangeItemAsync(ItemId oldItemId, IStoreItem newItem, CancellationToken cancellationToken);
}