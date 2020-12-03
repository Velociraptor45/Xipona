using ShoppingList.Frontend.Models.Items;
using System;

namespace ShoppingList.Frontend.Models.Shared.Requests
{
    public class UpdateItemRequest : IApiRequest
    {
        public UpdateItemRequest(Guid requestId, StoreItem storeItem)
        {
            RequestId = requestId;
            StoreItem = storeItem;
        }

        public Guid RequestId { get; }
        public StoreItem StoreItem { get; }
    }
}