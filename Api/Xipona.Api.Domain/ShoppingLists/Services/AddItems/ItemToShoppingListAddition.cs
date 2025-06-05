using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.ShoppingLists.Services.AddItems;

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