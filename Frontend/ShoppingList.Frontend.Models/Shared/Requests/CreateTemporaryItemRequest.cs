using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class CreateTemporaryItemRequest : IApiRequest
    {
        public CreateTemporaryItemRequest(Guid requestId, Guid offlineId, string name, int storeId, float price,
            int defaultSectionId)
        {
            RequestId = requestId;
            OfflineId = offlineId;
            Name = name;
            StoreId = storeId;
            Price = price;
            DefaultSectionId = defaultSectionId;
        }

        public Guid RequestId { get; }
        public Guid OfflineId { get; }
        public string Name { get; }
        public int StoreId { get; }
        public float Price { get; }
        public int DefaultSectionId { get; }
    }
}