using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;

public class ShoppingListItemFactory : IShoppingListItemFactory
{
    public IShoppingListItem Create(ItemId id, ItemTypeId? typeId, bool isInBasket, QuantityInBasket quantity)
    {
        return new ShoppingListItem(
            id,
            typeId,
            isInBasket,
            quantity);
    }
}