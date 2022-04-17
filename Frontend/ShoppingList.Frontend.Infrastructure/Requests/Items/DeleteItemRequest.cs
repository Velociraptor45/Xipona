using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class DeleteItemRequest : IApiRequest
    {
        public DeleteItemRequest(Guid requestId, Guid itemId)
        {
            RequestId = requestId;
            ItemId = itemId;
        }

        public Guid RequestId { get; }
        public Guid ItemId { get; }
    }
}