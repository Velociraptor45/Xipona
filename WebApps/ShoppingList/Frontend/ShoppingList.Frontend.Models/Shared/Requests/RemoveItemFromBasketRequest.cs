using System;

namespace ShoppingList.Frontend.Models.Shared.Requests
{
    public class RemoveItemFromBasketRequest : IApiRequest
    {
        public RemoveItemFromBasketRequest(Guid requestId, int shoppingListId, ItemId itemId)
        {
            RequestId = requestId;
            ShoppingListId = shoppingListId;
            ItemId = itemId;
        }

        public Guid RequestId { get; }
        public int ShoppingListId { get; }
        public ItemId ItemId { get; }
    }
}