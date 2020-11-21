using System;

namespace ShoppingList.Frontend.Models.Shared.Requests
{
    public class AddItemToShoppingListRequest : IApiRequest
    {
        public AddItemToShoppingListRequest(Guid requestId, int shoppingListId, int itemId, float quantity)
        {
            RequestId = requestId;
            ShoppingListId = shoppingListId;
            ItemId = itemId;
            Quantity = quantity;
        }

        public Guid RequestId { get; }
        public int ShoppingListId { get; }
        public int ItemId { get; }
        public float Quantity { get; }
    }
}