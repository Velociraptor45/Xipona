using ShoppingList.Frontend.Models.Items;
using System;

namespace ShoppingList.Frontend.Models.Shared.Requests
{
    public class ChangeItemRequest : IApiRequest
    {
        public ChangeItemRequest(Guid requestId, StoreItem storeItem)
        {
            RequestId = requestId;
            StoreItem = storeItem;
        }

        public Guid RequestId { get; }
        public StoreItem StoreItem { get; }
    }
}