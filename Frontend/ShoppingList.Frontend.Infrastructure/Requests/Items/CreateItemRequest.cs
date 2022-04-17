using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Items
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