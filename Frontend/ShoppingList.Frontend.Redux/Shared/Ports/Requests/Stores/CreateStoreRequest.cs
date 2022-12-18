using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;

namespace ShoppingList.Frontend.Redux.Shared.Ports.Requests.Stores
{
    public class CreateStoreRequest : IApiRequest
    {
        public CreateStoreRequest(Guid requestId, Store store)
        {
            RequestId = requestId;
            Store = store;
        }

        public Guid RequestId { get; }
        public Store Store { get; }
    }
}