using System;

namespace ShoppingList.Frontend.Models.Shared.Requests
{
    public class PutItemInBasketRequest : IApiRequest
    {
        public PutItemInBasketRequest(Guid requestId, int shoppingListId, int itemId)
        {
            RequestId = requestId;
            ShoppingListId = shoppingListId;
            ItemId = itemId;
        }

        public Guid RequestId { get; }
        public int ShoppingListId { get; }
        public int ItemId { get; }
    }
}