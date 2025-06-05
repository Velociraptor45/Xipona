using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.ShoppingLists.Services.Exchanges;

public interface IShoppingListExchangeService
{
    Task ExchangeItemAsync(ItemId oldItemId, IItem newItem);
}