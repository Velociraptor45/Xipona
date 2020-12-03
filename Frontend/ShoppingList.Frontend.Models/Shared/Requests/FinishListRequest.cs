using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class FinishListRequest : IApiRequest
    {
        public FinishListRequest(Guid requestId, int shoppingListId)
        {
            RequestId = requestId;
            ShoppingListId = shoppingListId;
        }

        public Guid RequestId { get; }
        public int ShoppingListId { get; }
    }
}