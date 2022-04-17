using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ShoppingLists
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