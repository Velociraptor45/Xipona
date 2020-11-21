using System;

namespace ShoppingList.Frontend.Models.Shared.Requests
{
    public class DeleteItemRequest : IApiRequest
    {
        public DeleteItemRequest(Guid requestId, int itemId)
        {
            RequestId = requestId;
            ItemId = itemId;
        }

        public Guid RequestId { get; }
        public int ItemId { get; }
    }
}