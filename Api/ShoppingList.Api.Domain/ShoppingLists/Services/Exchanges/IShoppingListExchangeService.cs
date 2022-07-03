using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Exchanges;

public interface IShoppingListExchangeService
{
    Task ExchangeItemAsync(ItemId oldItemId, IItem newItem, CancellationToken cancellationToken);
}