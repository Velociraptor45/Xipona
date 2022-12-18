using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;

namespace ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items
{
    public class CreateItemWithTypesRequest : IApiRequest
    {
        public CreateItemWithTypesRequest(Guid requestId, Item storeItem)
        {
            RequestId = requestId;
            StoreItem = storeItem;
        }

        public Guid RequestId { get; }
        public Item StoreItem { get; }
    }
}