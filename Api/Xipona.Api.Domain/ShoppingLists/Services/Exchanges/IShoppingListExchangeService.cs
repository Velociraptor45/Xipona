using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Exchanges;

public interface IShoppingListExchangeService
{
    Task ExchangeItemAsync(ItemId oldItemId, IItem newItem);
}