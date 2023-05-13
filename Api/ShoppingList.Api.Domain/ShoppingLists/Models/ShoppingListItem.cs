using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

public class ShoppingListItem : IShoppingListItem
{
    public ShoppingListItem(ItemId id, ItemTypeId? typeId, bool isInBasket, QuantityInBasket quantity)
    {
        Id = id;
        TypeId = typeId;
        IsInBasket = isInBasket;
        Quantity = quantity;
    }

    public ItemId Id { get; }
    public ItemTypeId? TypeId { get; }
    public bool IsInBasket { get; }
    public QuantityInBasket Quantity { get; }

    public IShoppingListItem PutInBasket()
    {
        return new ShoppingListItem(Id, TypeId, true, Quantity);
    }

    public IShoppingListItem RemoveFromBasket()
    {
        return new ShoppingListItem(Id, TypeId, false, Quantity);
    }

    public IShoppingListItem ChangeQuantity(QuantityInBasket quantity)
    {
        return new ShoppingListItem(Id, TypeId, IsInBasket, quantity);
    }

    public IShoppingListItem AddQuantity(QuantityInBasket quantity)
    {
        return new ShoppingListItem(Id, TypeId, IsInBasket, Quantity + quantity);
    }
}