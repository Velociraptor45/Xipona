using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Exchanges;

public interface IShoppingListExchangeService
{
    Task ExchangeItemAsync(ItemId oldItemId, IItem newItem);
}