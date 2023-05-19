using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;

public class ItemToShoppingListAddition
{
    public ItemToShoppingListAddition(ItemId itemId, ItemTypeId? itemTypeId, StoreId storeId,
        QuantityInBasket quantity)
    {
        ItemId = itemId;
        ItemTypeId = itemTypeId;
        StoreId = storeId;
        Quantity = quantity;
    }

    public ItemId ItemId { get; }
    public ItemTypeId? ItemTypeId { get; }
    public StoreId StoreId { get; }
    public QuantityInBasket Quantity { get; }
}