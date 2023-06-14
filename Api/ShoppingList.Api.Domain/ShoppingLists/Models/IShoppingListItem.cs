using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

public interface IShoppingListItem
{
    ItemId Id { get; }
    bool IsInBasket { get; }
    QuantityInBasket Quantity { get; }
    ItemTypeId? TypeId { get; }

    IShoppingListItem PutInBasket();

    IShoppingListItem RemoveFromBasket();

    IShoppingListItem ChangeQuantity(QuantityInBasket quantity);

    IShoppingListItem AddQuantity(QuantityInBasket quantity);
}