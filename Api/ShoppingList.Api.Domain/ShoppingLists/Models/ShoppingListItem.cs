using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

public class ShoppingListItem : IShoppingListItem
{
    public ShoppingListItem(ItemId id, ItemTypeId? typeId, bool isInBasket, float quantity)
    {
        Id = id;
        TypeId = typeId;
        IsInBasket = isInBasket;
        Quantity = quantity;
    }

    public ItemId Id { get; }
    public ItemTypeId? TypeId { get; }
    public bool IsInBasket { get; }
    public float Quantity { get; }

    public IShoppingListItem PutInBasket()
    {
        return new ShoppingListItem(Id, TypeId, true, Quantity);
    }

    public IShoppingListItem RemoveFromBasket()
    {
        return new ShoppingListItem(Id, TypeId, false, Quantity);
    }

    public IShoppingListItem ChangeQuantity(float quantity)
    {
        return new ShoppingListItem(Id, TypeId, IsInBasket, quantity);
    }
}