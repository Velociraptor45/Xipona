using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class FinishListRequest : IApiRequest
    {
        public FinishListRequest(Guid requestId, Guid shoppingListId)
        {
            RequestId = requestId;
            ShoppingListId = shoppingListId;
        }

        public Guid RequestId { get; }
        public Guid ShoppingListId { get; }
    }
}