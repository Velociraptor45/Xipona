using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class ChangeItemQuantityOnShoppingListRequest : IApiRequest
    {
        public ChangeItemQuantityOnShoppingListRequest(Guid requestId, int shoppingListId, ItemId itemId,
            int? itemTypeId, float quantity)
        {
            RequestId = requestId;
            ShoppingListId = shoppingListId;
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            Quantity = quantity;
        }

        public Guid RequestId { get; }
        public int ShoppingListId { get; }
        public ItemId ItemId { get; }
        public int? ItemTypeId { get; }
        public float Quantity { get; }
    }
}