using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class PutItemInBasketRequest : IApiRequest
    {
        public PutItemInBasketRequest(Guid requestId, int shoppingListId, ItemId itemId)
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