using System;

namespace ShoppingList.Frontend.Models.Shared.Requests
{
    public class CreateTemporaryItemRequest : IApiRequest
    {
        public CreateTemporaryItemRequest(Guid requestId, Guid offlineId, string name, int storeId, float price)
        {
            RequestId = requestId;
            OfflineId = offlineId;
            Name = name;
            StoreId = storeId;
            Price = price;
        }

        public Guid RequestId { get; }
        public Guid OfflineId { get; }
        public string Name { get; }
        public int StoreId { get; }
        public float Price { get; }
    }
}