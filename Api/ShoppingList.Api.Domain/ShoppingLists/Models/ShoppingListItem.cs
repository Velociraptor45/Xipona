using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

public record ShoppingListItem(ItemId Id, ItemTypeId? TypeId, bool IsInBasket, QuantityInBasket Quantity)
{
    public ShoppingListItem PutInBasket()
    {
        return this with { IsInBasket = true };
    }

    public ShoppingListItem RemoveFromBasket()
    {
        return this with { IsInBasket = false };
    }

    public ShoppingListItem ChangeQuantity(QuantityInBasket quantity)
    {
        return this with { Quantity = quantity };
    }

    public ShoppingListItem AddQuantity(QuantityInBasket quantity)
    {
        return this with { Quantity = Quantity + quantity };
    }
}