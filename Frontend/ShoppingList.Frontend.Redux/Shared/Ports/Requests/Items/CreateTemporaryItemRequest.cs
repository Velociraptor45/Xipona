namespace ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items
{
    public class CreateTemporaryItemRequest : IApiRequest
    {
        public CreateTemporaryItemRequest(Guid requestId, Guid offlineId, string name, Guid storeId, float price,
            Guid defaultSectionId)
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
        public Guid StoreId { get; }
        public float Price { get; }
        public Guid DefaultSectionId { get; }
    }
}