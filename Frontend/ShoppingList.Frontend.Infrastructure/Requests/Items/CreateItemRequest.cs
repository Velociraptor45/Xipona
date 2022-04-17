using ProjectHermes.ShoppingList.Frontend.Models.Items;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class CreateItemRequest : IApiRequest
    {
        public CreateItemRequest(Guid requestId, Item storeItem)
        {
            RequestId = requestId;
            StoreItem = storeItem;
        }

        public Guid RequestId { get; }
        public Item StoreItem { get; }
    }
}